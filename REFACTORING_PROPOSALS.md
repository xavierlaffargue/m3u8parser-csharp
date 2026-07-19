# Propositions de Refactorisation de la Bibliothèque M3U8Parser

Ce document présente une analyse détaillée des opportunités de refactorisation, de nettoyage et de modernisation de la bibliothèque `M3U8Parser`. Ces propositions visent à améliorer la **maintenabilité**, la **robustesse**, la **lisibilité** et l'**évolutivité** du code, tout en garantissant une **rétrocompatibilité ascendante stricte** (aucune modification de signature publique cassante) et en tirant parti des nouveautés des versions récentes de C#.

---

## Sommaire
1. [Modernisation de la syntaxe C# (C# 12 / 13)](#1-modernisation-de-la-syntaxe-c-c-12--13)
2. [Amélioration de la robustesse et Null-Safety](#2-amélioration-de-la-robustesse-et-null-safety)
3. [Optimisation de l'architecture de parsing des attributs (Single-Pass Parsing)](#3-optimisation-de-larchitecture-de-parsing-des-attributs-single-pass-parsing)
4. [Élimination de la réflexion dynamique non mise en cache (Source Generators ou Cache Statique)](#4-élimination-de-la-réflexion-dynamique-non-mise-en-cache-source-generators-ou-cache-statique)
5. [Refactorisation et découpage des méthodes de chargement massives (`MediaPlaylist.LoadFromText`)](#5-refactorisation-et-découpage-des-méthodes-de-chargement-massives-mediaplaylistloadfromtext)
6. [Améliorations de performance légères et gestion de chaînes (`StringBuilder`)](#6-améliorations-de-performance-légères-et-gestion-de-chaînes-stringbuilder)
7. [Résolution des avertissements de style et de compilation](#7-résolution-des-avertissements-de-style-et-de-compilation)

---

## 1. Modernisation de la syntaxe C# (C# 12 / 13)

### A. Expressions de collection (Collection Expressions)
Le code actuel utilise des instanciations de listes traditionnelles ou des tableaux vides :
```csharp
List<Media> medias = new ();
var str = m.Invoke(..., System.Array.Empty<object>());
```
**Proposition :** Utiliser la syntaxe simplifiée de C# 12 :
```csharp
List<Media> medias = [];
var str = m.Invoke(..., []);
```
* **Avantage :** Syntaxe plus concise, uniforme, et optimisée par le compilateur (génération de tableaux statiques ou d'allocations optimisées en coulisses).

### B. Constructeurs Primaires (Primary Constructors)
De nombreuses classes d'attributs n'ont qu'un constructeur simple qui affecte un paramètre à une propriété ou un champ en lecture seule.
Exemple actuel (`BoolAttribute.cs`) :
```csharp
public class BoolAttribute : CustomAttribute<bool>
{
    public BoolAttribute(string attributeName) : base(attributeName) { }
}
```
**Proposition :** Utiliser des constructeurs primaires de classe :
```csharp
public class BoolAttribute(string attributeName) : CustomAttribute<bool>(attributeName)
{
    // ...
}
```
* **Avantage :** Réduction du code répétitif (boilerplate code) et lisibilité accrue.

---

## 2. Amélioration de la robustesse et Null-Safety

### A. Activation du contexte Nullable (`<Nullable>enable</Nullable>`)
Actuellement, les types de référence nullable ne sont pas activés. Cela rend la bibliothèque sujette à de potentielles `NullReferenceException` silencieuses (par exemple, si `Map` ou `IndependentSegments` sont accédés alors qu'ils n'ont pas été instanciés).

**Proposition :**
1. Activer `<Nullable>enable</Nullable>` dans `M3U8Parser.csproj`.
2. Annoter proprement les propriétés publiques avec `?` (ex: `public Map? Map { get; set; }`).
3. Compiler et corriger tous les avertissements de flux de données de nullité.
* **Avantage :** Sécurité de type au moment de la compilation, documentation claire des API quant aux valeurs pouvant être nulles.

### B. Sécurisation du parsing d'attributs (Éliminer `Split('=')[1]`)
Le parsing actuel des attributs suppose que la chaîne extraite par Regex contient toujours un symbole `=` suivi d'une valeur :
```csharp
var valueFounded = match.Groups[0].Value.Split('=')[1];
```
Si le fichier M3U8 est malformé ou si l'attribut est écrit de manière inattendue, un `IndexOutOfRangeException` sera levé, faisant planter l'application cliente.

**Proposition :** Sécuriser l'extraction en validant la structure :
```csharp
var parts = match.Groups[0].Value.Split('=');
var valueFounded = parts.Length > 1 ? parts[1] : string.Empty;
```
* **Avantage :** Plus grande résilience face aux fichiers M3U8 corrompus, tout en respectant le principe de la bibliothèque de "ne pas tenter de valider le fichier selon la RFC mais d'exposer ce qu'elle trouve de manière robuste".

---

## 3. Optimisation de l'architecture de parsing des attributs (Single-Pass Parsing)

### A. Problématique actuelle
Pour analyser les attributs d'une ligne de tag (par exemple `EXT-X-STREAM-INF`), la bibliothèque exécute une expression régulière différente pour **chaque attribut individuel** déclaré dans la classe.
Si une classe possède 15 attributs, le code va instancier et exécuter 15 Regex sur la même ligne de texte !

### B. Proposition : Parsing en une seule passe (Single-Pass Tokenization)
Au lieu d'interroger chaque attribut pour qu'il cherche sa propre valeur via sa propre Regex, nous devrions découper la ligne d'attributs en paires clé-valeur (tokenisation) en une seule passe :
```csharp
public static Dictionary<string, string> ParseAttributes(string attributeString)
{
    var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    // Logique de tokenisation respectant les valeurs entre guillemets (ex: KEY="VALUE",KEY2=VALUE2)
    // ...
    return attributes;
}
```
Ensuite, chaque attribut n'a plus qu'à récupérer sa valeur directement dans le dictionnaire en temps constant $O(1)$ sans aucune Regex supplémentaire.

* **Avantage :** Énorme gain d'allocation mémoire, code beaucoup plus propre et élimination complète de dizaines de Regex complexes et difficiles à maintenir.

---

## 4. Élimination de la réflexion dynamique non mise en cache (Source Generators ou Cache Statique)

### A. Problématique actuelle
`AbstractTag` et `AbstractTagOneValue` utilisent la réflexion au moment de l'exécution pour trouver tous les champs privés héritant d'un certain type :
```csharp
foreach (var field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
{
    var isAnAttribute = typeof(IAttribute).IsAssignableFrom(field.FieldType);
    // ... Invoke Read ou ToString via Réflexion
}
```
Cette recherche d'attributs via réflexion est exécutée à **chaque fois** qu'un tag est instancié ou converti en chaîne, sans aucune mise en cache des métadonnées des types.

### B. Proposition 1 : Cache statique des champs de réflexion
Mettre en cache les informations de champs de manière statique au niveau de la classe de base pour éviter de réinterroger le runtime .NET à chaque objet :
```csharp
private static readonly ConcurrentDictionary<Type, FieldInfo[]> FieldsCache = new();

protected void ReadAllAttributes(string str)
{
    var fields = FieldsCache.GetOrAdd(GetType(), t =>
        t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
         .Where(f => typeof(IAttribute).IsAssignableFrom(f.FieldType))
         .ToArray());

    foreach (var field in fields)
    {
        var attributeInstance = field.GetValue(this) as IAttribute;
        attributeInstance?.Read(str);
    }
}
```
*Note : Cela élimine également le besoin d'appeler `GetMethod("Read")` puis `Invoke()`, car on peut simplement transtyper l'objet en `IAttribute` et appeler `.Read(str)` directement de manière fortement typée !*

### C. Proposition 2 : Générateurs de source (C# Source Generators)
Pour une solution ultra-performante et "AOT-friendly" (Ahead-Of-Time compilation) :
Utiliser des Source Generators pour générer automatiquement le code d'écriture/lecture des attributs au moment de la compilation. Cela élimine complètement la réflexion à l'exécution.

---

## 5. Refactorisation et découpage des méthodes de chargement massives (`MediaPlaylist.LoadFromText`)

### A. Problématique actuelle
La méthode `MediaPlaylist.LoadFromText` contient une boucle de lecture géante avec plus de 20 branches conditionnelles `else if (line.StartsWith(...))` et maintient de nombreuses variables d'état locales temporaires (`currentMap`, `currentProgramDateTime`, `currentKey`, `currentGap`, etc.).
Cette centralisation de la logique rend la maintenance difficile lors de l'ajout de nouveaux tags de segments média.

### B. Proposition : Pattern Strategy / Command pour les tags
Découper le parseur en enregistrant des processeurs de tags associés à des préfixes.
Exemple de structure conceptuelle :
```csharp
interface IMediaPlaylistLineProcessor
{
    bool CanProcess(string line);
    void Process(string line, MediaPlaylistParsingContext context);
}
```
Chaque type de tag (`EXT-X-KEY`, `EXT-X-MAP`, `EXTINF`, etc.) implémente cette interface dans son propre fichier. La boucle principale devient alors un simple parcours propre et extensible de processeurs enregistrés.

* **Avantage :** Isolation parfaite de la logique de chaque tag, code hautement extensible et possibilité de tester unitairement le comportement de parsing de chaque tag en isolation.

---

## 6. Améliorations de performance légères et gestion de chaînes (`StringBuilder`)

### A. Optimisation de la construction de chaîne dans `AbstractTag.ToString`
Actuellement, la génération de chaîne fait :
```csharp
public override string ToString()
{
    var strBuilder = new StringBuilder();
    strBuilder.Append(TagName);
    strBuilder.Append(':');
    WriteAllAttributes(strBuilder);
    return strBuilder.ToString().RemoveLastCharacter();
}
```
Cela alloue une chaîne via `strBuilder.ToString()`, puis crée une **nouvelle** chaîne via `RemoveLastCharacter()` pour supprimer le dernier séparateur `,`.

**Proposition :**
Modifier la méthode `WriteAllAttributes` pour qu'elle ajoute le séparateur uniquement *entre* les attributs (comme un `string.Join`), évitant ainsi d'ajouter un caractère en trop pour devoir le retirer juste après.
```csharp
protected void WriteAllAttributes(StringBuilder strBuilder)
{
    var fields = GetAttributeFields();
    bool first = true;
    foreach (var field in fields)
    {
        var attribute = field.GetValue(this) as IAttribute;
        var valueStr = attribute?.ToString();
        if (!string.IsNullOrEmpty(valueStr))
        {
            if (!first)
            {
                strBuilder.Append(',');
            }
            strBuilder.Append(valueStr);
            first = false;
        }
    }
}
```
* **Avantage :** Évite d'allouer une chaîne intermédiaire inutile et d'utiliser une méthode risquant des exceptions d'indexation (`RemoveLastCharacter`).

---

## 7. Résolution des avertissements de style et de compilation

Les avertissements StyleCop et du compilateur doivent être définitivement éliminés ou explicitement masqués s'ils sont non-pertinents.

### Actions déjà réalisées dans le cadre de ce plan :
1. **Avertissement CS0114 (ToString masque le membre hérité) :**
   Résolu de manière propre et rétrocompatible en remplaçant la déclaration masquante `public new string ToString()` ou `public virtual string ToString()` par un véritable `public override string ToString()` dans `AbstractTag`, `AbstractTagOneValue` et `AbstractTagWithoutValue`.
2. **Avertissement StyleCop SA1201 (L'ordre des membres de classe) :**
   Résolu dans `AbstractTagWithoutValue.cs` en repositionnant les constructeurs avant la propriété publique `IsPresent`, garantissant le respect strict des règles d'analyse statique du projet.

---

## Synthèse & Recommandations de Priorité

| Recommandation | Complexité | Impact Maintenance | Impact Performance | Priorité |
|---|---|---|---|---|
| **Résolution des Warnings (SA1201 / CS0114)** | Très Faible | Faible | Neutre | **Haute** (Déjà fait) |
| **Sécurisation du Split (`Split('=')[1]`)** | Faible | Élevé | Neutre | **Haute** |
| **Cache de Réflexion Statique dans `AbstractTag`** | Faible | Élevé | Moyen | **Moyenne** |
| **Activation des Nullable Reference Types** | Moyenne | Élevé | Neutre | **Moyenne** |
| **Single-Pass Attribute Parsing** | Moyenne | Élevé | Élevé | **Moyenne** |
| **Refactorisation de `MediaPlaylist.LoadFromText`** | Élevée | Très Élevé | Neutre | **Basse** |
| **Source Generators** | Élevée | Élevé | Très Élevé | **Basse** |

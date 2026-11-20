// Locked-properties example: EFDS copies of Atrio's WoodTrimKit (parent, with Metallic Map + Normal Map locked) and a
// variant overriding Base Color, in Assets/EFDS_Screenshots/Materials. Atrio's own assets are not modified.
using UnityEditor; using UnityEngine; using System.Linq;
string DIR = "Assets/EFDS_Screenshots/Materials", P = DIR + "/WoodTrimKit Parent.mat", V = DIR + "/WoodTrimKit Variant.mat";
if (!AssetDatabase.IsValidFolder(DIR)) AssetDatabase.CreateFolder("Assets/EFDS_Screenshots", "Materials");
if (AssetDatabase.LoadAssetAtPath<Material>(P) == null) AssetDatabase.CopyAsset("Assets/Atrio/Environment/WoodTrimKit/Materials/WoodTrimKit.mat", P);
var parent = AssetDatabase.LoadAssetAtPath<Material>(P);
var variant = AssetDatabase.LoadAssetAtPath<Material>(V);
if (variant == null) { variant = new Material(parent) { parent = parent }; AssetDatabase.CreateAsset(variant, V); }
variant.parent = parent;
variant.SetColor("_BaseColor", new Color(0.95f, 0.6f, 0.58f));
foreach (var n in new[]{ "_MetallicGlossMap", "_Metallic", "_BumpMap", "_BumpScale" }) if (parent.HasProperty(n)) parent.SetPropertyLock(n, true);
EditorUtility.SetDirty(parent); EditorUtility.SetDirty(variant); AssetDatabase.SaveAssets();
return "locked: " + string.Join(",", new[]{ "_MetallicGlossMap", "_Metallic", "_BumpMap", "_BumpScale" }.Where(n => variant.IsPropertyLockedByAncestor(n)));

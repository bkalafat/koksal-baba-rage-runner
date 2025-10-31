using UnityEngine;

namespace KoksalBaba.Data
{
    /// <summary>
    /// ScriptableObject defining biome theme (background, palette, music).
    /// </summary>
    [CreateAssetMenu(fileName = "Theme", menuName = "Rage Runner/Theme")]
    public class Theme : ScriptableObject
    {
        [Header("Biome Identity")]
        public string themeName = "Street";

        [Header("Visuals")]
        public Sprite backgroundSprite;
        public Color primaryColor = Color.gray;
        public Color secondaryColor = Color.white;

        [Header("Audio")]
        public AudioClip backgroundMusic;

        [Header("Obstacle Variants (optional)")]
        public GameObject[] obstacleVariants;
    }
}

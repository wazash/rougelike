using TMPro;
using UnityEngine;

namespace TMPLorem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPWithLorem : MonoBehaviour
    {
        private const int max_words = 50;
        private const int min_words = 5;

        [SerializeField, Range(1, 5)] private int paragraphs = 1;
        [SerializeField, Range(1, 10)] private int sentencesPerParagraph = 2;
        [HideInInspector] public MinMaxInt WordsAmount = new(min_words, min_words + 5);

        private TextMeshProUGUI textMeshPro;

        public int Min_words => min_words;

        public int Max_words => max_words;

        private void Awake()
        {
            if (!TryGetComponent(out textMeshPro))
            {
                Debug.LogError("TMPWithLorem requires a TextMeshProUGUI component on the same GameObcjet.");
            }
        }

        private void OnValidate()
        {
            if (!TryGetComponent(out textMeshPro))
            {
                Debug.LogError("TMPWithLorem requires a TextMeshProUGUI component on the same GameObcjet.");
            }
        }

        public void GenerateLorem()
        {
            if (textMeshPro != null)
            {
                string loremText = LoremIpsum.Generate(paragraphs, sentencesPerParagraph, WordsAmount);
                textMeshPro.text = loremText;
            }
        }

        public void Clear()
        {
            if (textMeshPro != null)
            {
                string loremText = string.Empty;
                textMeshPro.text = loremText;
            }
        }
    }
}

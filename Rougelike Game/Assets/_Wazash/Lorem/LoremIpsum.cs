using System.Text;
using UnityEngine;

namespace TMPLorem
{
    public static class LoremIpsum
    {
        private static readonly string[] sampleWords = {
        "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit",
        "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore", "magna", "aliqua",
        "enim", "minim", "veniam", "quis", "nostrud", "exercitation", "ullamco", "laboris", "nisi",
        "aliquip", "ex", "ea", "commodo", "consequat", "duis", "aute", "irure", "in", "reprehenderit",
        "voluptate", "velit", "esse", "cillum", "eu", "fugiat", "nulla", "pariatur",
        "excepteur", "sint", "occaecat", "cupidatat", "non", "proident", "sunt", "culpa",
        "qui", "officia", "deserunt", "mollit", "anim", "id", "est", "laborum", "praesentium", "voluptatum",
        "deleniti", "atque", "corrupti", "quos", "dolores", "quas", "molestias", "excepturi",
        "obcaecati", "cupiditate", "provident", "similique", "mollitia", "animi", "harum", "quidem", "rerum", "facilis",
        "expedita", "distinctio", "nam", "libero", "tempore", "cum", "soluta", "nobis", "eligendi", "optio", "cumque", "nihil",
        "impedit", "quo", "minus", "quod", "maxime", "placeat", "facere", "possimus", "omnis",
        "voluptas", "assumenda", "repellendus", "temporibus", "autem", "quibusdam", "aut", "consequatur", "vel", "illum",
        "fugiat", "pariatur"
    };

        public static string Generate(int paragraphs, int sentencesPerParagraph, MinMaxInt wordsRange)
        {
            StringBuilder result = new();

            for (int i = 0; i < paragraphs; i++)
            {
                for (int j = 0; j < sentencesPerParagraph; j++)
                {
                    StringBuilder sentence = new();
                    int wordsPerSentence = Random.Range(wordsRange.MinValue, wordsRange.MaxValue);

                    for (int k = 0; k < wordsPerSentence; k++)
                    {
                        if (i == 0 && j == 0 && k < 5)
                        {
                            sentence.Append(sampleWords[k]);

                            if (k == 1)
                                sentence.Append(", ");
                            else if
                                (k < 4) sentence.Append(" ");

                            continue;
                        }

                        string word = sampleWords[Random.Range(0, sampleWords.Length)];

                        if (k > 0)
                            sentence.Append(' ');

                        sentence.Append(word);

                        if (k < wordsPerSentence - 1 && Random.Range(0, 5) == 0)
                            sentence.Append(",");
                    }

                    string finishedSentence = sentence.ToString();
                    finishedSentence = char.ToUpper(finishedSentence[0]) + finishedSentence.Substring(1);
                    result.Append(finishedSentence);

                    if (j < sentencesPerParagraph - 1 || i < paragraphs - 1)
                        result.Append(". ");
                }

                if (i < paragraphs - 1)
                    result.Append("\n\n");
            }

            return result.ToString();
        }
    }
}

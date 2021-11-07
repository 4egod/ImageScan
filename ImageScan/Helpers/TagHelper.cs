namespace ImageScan
{
    internal static class TagHelper
    {
        public static string GetTag(string content)
        {
            int i = content.IndexOf("image:");

            if (i == -1)
            {
                throw new ArgumentException(content, nameof(content));
            }

            i += 6;

            int start = content.IndexOf(":", i);
            start++;

            int end = content.IndexOf("\n", start);

            string tag = content[start..end];

            i = tag.IndexOf("#");

            if (i != -1)
            {
                tag = tag[..i];
            }

            tag = tag.TrimEnd();

            return tag;
        }

        public static string ChangeTag(string content, string newTag)
        {
            int i = content.IndexOf("image:");

            if (i == -1)
            {
                throw new ArgumentException(content, nameof(content));
            }

            i += 6;

            int start = content.IndexOf(":", i);
            start++;

            int end = content.IndexOf("\n", start);

            content = content.Remove(start, end - start);

            content = content.Insert(start, newTag);

            return content;
        }
    }
}

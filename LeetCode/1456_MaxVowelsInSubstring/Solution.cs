public class Solution
{
    public int MaxVowels(string s, int k)
    {
        int sum = 0;
        for (int i = 0; i < k; i++) //O(n)
        {
            if (s[i] == 'a' || s[i] == 'i' || s[i] == 'e' || s[i] == 'o' || s[i] == 'u')
            {
                sum++;
            }
        }
        int maxCurrent = sum;


        for (int end = k; end < s.Length; end++) //O(n)
        {
            int left = end - k;

            if (s[left] == 'a' || s[left] == 'i' || s[left] == 'e' || s[left] == 'o' || s[left] == 'u')
            { sum--; }
            if (s[end] == 'a' || s[end] == 'i' || s[end] == 'e' || s[end] == 'o' || s[end] == 'u')
            { sum++; }

            if (sum > maxCurrent) { maxCurrent = sum; }
        }


        return maxCurrent;

    }
}
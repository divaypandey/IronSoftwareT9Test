using System.Text;

namespace T9Decoder.Services
{
    public static class T9TextService
    {
        private static readonly Dictionary<int, char[]> t9CharKeys = new()
        {
            {0, [' '] },
            {1,['&', '\'', '('] },
            {2,['a', 'b', 'c'] },
            {3,['d', 'e', 'f'] },
            {4,['g', 'h', 'i'] },
            {5,['j', 'k', 'l'] },
            {6,['m', 'n', 'o'] },
            {7,['p', 'q', 'r', 's'] },
            {8,['t', 'u', 'v'] },
            {9,['w', 'x', 'y', 'z'] },
        };
        private static readonly char[] specialChars = ['*', '#', ' '];

        public static string T9ToString(string t9str)
        {
            try
            {
                int keyInFocus = -1;
                int keyPressedTimes = 0;
                StringBuilder toReturn = new();

                foreach (char c in t9str)
                {
                    if (int.TryParse($"{c}", out int keypadDigit)) //if input is one of the digit-keypad keys
                    {
                        if (keypadDigit < 0) continue;

                        if (keyInFocus == -1) //beginning of input
                        {
                            keyInFocus = keypadDigit;
                            keyPressedTimes = 1;
                        }
                        else
                        {
                            if (keyInFocus == keypadDigit) keyPressedTimes++;
                            else //key change, process what we have till now
                            {
                                int keyLen = keyInFocus >= 0 ? t9CharKeys[keyInFocus].Length : 0;
                                int rotationMod = keyLen > 0 ? keyPressedTimes % keyLen : 1; //Mod checks KeyLen to avoid divide-by-0 error
                                keyPressedTimes = keyPressedTimes > keyLen ? (rotationMod == 0 ? keyLen : rotationMod) : keyPressedTimes;
                                //if mod is 0 means user pressed the key same as length of chars in it, i.e., if i press 222 i expect C, if i press 222222, it should rotate to C

                                if (keyInFocus >= 0) toReturn.Append(t9CharKeys[keyInFocus][keyPressedTimes - 1]);
                                keyInFocus = keypadDigit;
                                keyPressedTimes = 1;
                            }
                        }
                    }
                    else if (specialChars.Contains(c)) //special
                    {
                        int keyLen = keyInFocus >= 0 ? t9CharKeys[keyInFocus].Length : 0;
                        int rotationMod = keyLen > 0 ? keyPressedTimes % keyLen : 1; //Mod checks KeyLen to avoid divide-by-0 error
                        keyPressedTimes = keyPressedTimes > keyLen ? (rotationMod == 0 ? keyLen : rotationMod) : keyPressedTimes;

                        if (char.IsWhiteSpace(c) && keyPressedTimes > 0) toReturn.Append(t9CharKeys[keyInFocus][keyPressedTimes - 1]);
                        else if (c == '*')
                        {
                            if (keyInFocus >= 0) toReturn.Append(t9CharKeys[keyInFocus][keyPressedTimes - 1]); //process
                            if (toReturn.Length > 0) toReturn.Remove(toReturn.Length - 1, 1); //then delete the last char
                        }
                        else if (c == '#' && keyPressedTimes > 0)
                        {
                            toReturn.Append(t9CharKeys[keyInFocus][keyPressedTimes - 1]);
                            break; //process and exit, anything beyond 1st # is ignored
                        }

                        keyInFocus = -1;
                        keyPressedTimes = 0;
                    }
                    //any other input is ignored
                }

                return toReturn.ToString();
            }
            catch //assuming production, so no error displayed IF any occurs
            {
                return string.Empty;
            }
        }
    }
}

﻿namespace Poker.Web
{
    public static class ViewDataFormatter
    {
        public static string GameResult(int result)
        {
            return result == 0 ? "=": result > 0 ? "+" + result.ToString() : result.ToString();
        }
    }
}
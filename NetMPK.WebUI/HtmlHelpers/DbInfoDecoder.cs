namespace NetMPK.WebUI.HtmlHelpers
{
    public static class DbInfoDecoder
    {
        public static string DecodeDbExpr(string expr)
        {
            string output = "";
            switch (expr)
            {
                case "BUS":
                    output = "autobus";
                    break;
                case "TRAM":
                    output = "tramwaj";
                    break;
                case "CITY":
                    output = "miejska";
                    break;
                case "AGGL":
                    output = "aglomeracyjna";
                    break;
                case "DAY":
                    output = "dzienna";
                    break;
                case "NIGHT":
                    output = "nocna";
                    break;
                case "NORMAL":
                    output = "zwykła";
                    break;
                case "AUX":
                    output = "pomocnicza";
                    break;
                case "ACCEL":
                    output = "pośpieszna";
                    break;
                case "REPLACE":
                    output = "Zastępcza";
                    break;
            }
            return output;
        }
    }
}
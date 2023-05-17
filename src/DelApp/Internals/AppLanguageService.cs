using System.Collections.Generic;
using System.Globalization;

namespace DelApp.Locals
{
    internal static class AppLanguageService
    {
        private static readonly Dictionary<string, IAppLanguageProvider> s_dictLanguageName;
        private static readonly Dictionary<int, IAppLanguageProvider> s_dictLCID;
        private static readonly IAppLanguageProvider s_defaultLan;

        private static IAppLanguageProvider s_provider;

        static AppLanguageService()
        {
            s_dictLanguageName = new Dictionary<string, IAppLanguageProvider>();
            s_dictLCID = new Dictionary<int, IAppLanguageProvider>();
            s_defaultLan = AppLanguageProviderEn.Instance;
        }

        public static IAppLanguageProvider LanguageProvider
        {
            get
            {
                if (s_provider == null)
                {
                    CultureInfo lang = CultureInfo.CurrentCulture;
                    s_provider = s_dictLCID.TryGetValue(lang.LCID, out IAppLanguageProvider p) ||
                                s_dictLanguageName.TryGetValue(lang.TwoLetterISOLanguageName, out p) ?
                                    p :
                                    s_defaultLan;
                }
                return s_provider;
            }

        }

        public static void RegisterLanguageProvider(IAppLanguageProvider lp)
        {
            if (!s_dictLanguageName.ContainsKey(lp.TwoLetterISOLanguageName))
                s_dictLanguageName.Add(lp.TwoLetterISOLanguageName, lp);
            if (!s_dictLCID.ContainsKey(lp.LCID))
                s_dictLCID.Add(lp.LCID, lp);
        }

    }
}

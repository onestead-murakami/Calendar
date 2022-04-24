using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Calendar {
    public static class Holiday {
        //一般的な祝日（種別, 月, 日or週, 開始年, 終了年, 祝日名）
        private struct H {
            public int tp;
            public int mt;
            public int dw;
            public int ys;
            public int ye;
            public string nm;

            public H(int tp, int mt, int dw, int ys, int ye, string nm) {
                this.tp = tp;
                this.mt = mt;
                this.dw = dw;
                this.ys = ys;
                this.ye = ye;
                this.nm = nm;
            }
        }

        //種別（一般的な祝日）
        private const int TF = 1;//"fixed"
        private const int TH = 2;//"happy";
        private const int TS = 3;//"spring";
        private const int TA = 4;//"autumn";

        //不変的な祝日
        private const string NF = "振替休日";
        private const string NK = "国民の休日";

        //一般的な祝日
        private static H[] holidays = {
            new H(TF, 1, 1, 1949, 9999, "元日")
            , new H(TF, 1, 15, 1949, 1999, "成人の日")
            , new H(TH, 1, 2, 2000, 9999, "成人の日")
            , new H(TF, 2, 11, 1967, 9999, "建国記念の日")
            , new H(TF, 2, 23, 2020, 9999, "天皇誕生日")
            , new H(TS, 3, 0, 1949, 9999, "春分の日")
            , new H(TF, 4, 29, 1949, 1988, "天皇誕生日")
            , new H(TF, 4, 29, 1989, 2006, "みどりの日")
            , new H(TF, 4, 29, 2007, 9999, "昭和の日")
            , new H(TF, 5, 3, 1949, 9999, "憲法記念日")
            , new H(TF, 5, 4, 1988, 2006, NK)//３日と５日に挟まれている
            , new H(TF, 5, 4, 2007, 9999, "みどりの日")
            , new H(TF, 5, 5, 1949, 9999, "こどもの日")
            , new H(TH, 7, 3, 2003, 2019, "海の日")
            , new H(TF, 7, 20, 1996, 2002, "海の日")
            , new H(TF, 7, 23, 2020, 2020, "海の日")
            , new H(TF, 7, 22, 2021, 2021, "海の日")
            , new H(TH, 7, 3, 2022, 9999, "海の日")
            , new H(TF, 8, 11, 2016, 2019, "山の日")
            , new H(TF, 8, 10, 2020, 2020, "山の日")
            , new H(TF, 8, 8, 2021, 2021, "山の日")
            , new H(TF, 8, 11, 2022, 9999, "山の日")
            , new H(TA, 9, 0, 1948, 9999, "秋分の日")
            , new H(TF, 9, 15, 1966, 2002, "敬老の日")
            , new H(TH, 9, 3, 2003, 9999, "敬老の日")
            , new H(TF, 10, 10, 1966, 1999, "体育の日")
            , new H(TH, 10, 2, 2000, 2019, "体育の日")
            , new H(TF, 7, 24, 2020, 2020, "スポーツの日")
            , new H(TF, 7, 23, 2021, 2021, "スポーツの日")
            , new H(TH, 10, 2, 2022, 9999, "スポーツの日")
            , new H(TF, 11, 3, 1948, 9999, "文化の日")
            , new H(TF, 11, 23, 1948, 9999, "勤労感謝の日")
            , new H(TF, 12, 23, 1989, 2018, "天皇誕生日")
            //以下、1年だけの祝日
            , new H(TF, 4, 10, 1959, 1959, "皇太子明仁親王の結婚の儀")
            , new H(TF, 2, 24, 1989, 1989, "昭和天皇の大喪の礼")
            , new H(TF, 11, 12, 1990, 1990, "即位礼正殿の儀")
            , new H(TF, 6, 9, 1993, 1993, "皇太子徳仁親王の結婚の儀")
            , new H(TF, 5, 1, 2019, 2019, "即位の日")
            , new H(TF, 10, 22, 2019, 2019, "即位礼正殿の儀")
    };

        private const int SUNDAY = 0;
        private const int MONDAY = 1;

        private static string CheckHoliday(int year, int month, int day) {
            DateTime datetime;
            if (!DateTime.TryParseExact(
                $"{year:d4}{month:d2}{day:d2}", "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out datetime))
                return String.Empty;

            int week = Basic.GetFirstWeek(year, month);
            for (int i = 0; i < holidays.Length; i++) {
                H h = holidays[i];
                if (h.ys <= year && h.ye >= year) {
                    if (h.mt == month) {
                        switch (h.tp) {
                            case TF:
                                if (h.dw == day) return h.nm;
                                break;
                            case TH:
                                int week2 = week;
                                int count = 0;
                                for (int n = 1, c = 0; c < h.dw; n++, count++)
                                    if ((week2++ % 7) == MONDAY)
                                        c++;
                                if (count == day) return h.nm;
                                break;
                            case TS:
                                int d2 = (int)(20.8431 + 0.242194 * (year - 1980)) - ((year - 1980) / 4);
                                if (d2 == day) return h.nm;
                                break;
                            case TA:
                                int d3 = (int)(23.2488 + 0.242194 * (year - 1980)) - ((year - 1980) / 4);
                                if (d3 == day) return h.nm;
                                break;
                            default:
                                break;
                        }
                    } else { /** 対象月ではない **/}
                } else { /** 対象年ではない **/}
            }
            return null;
        }

        public static bool IsHoliday(int year, int month, int day) {
            return GetHoliday(year, month).ContainsKey(day);
        }

        public static List<Dictionary<int, string>> GetHoliday(int year) {
            var list = new List<Dictionary<int, string>>();
            for (int i = 1; i <= 12; i++)
                list.Add(GetHoliday(year, i));
            return list;
        }

        public static Dictionary<int, string> GetHoliday(int year, int month) {
            var map = new Dictionary<int, string>();

            int last = Basic.GetLastDay(year, month);
            int week = Basic.GetFirstWeek(year, month);

            for (int d = 1; d <= last; d++) {
                string h = CheckHoliday(year, month, d);
                if (h != null) map.Add(d, h);
            }
            if (1973 < year || (1973 == year && 4 <= month)) {
                int start = (1973 == year && 4 == month) ? 12 : 1;
                for (int d = start; d <= last; d++) {
                    int dayOfWeek = (week + d - 1) % 7;
                    if (dayOfWeek == SUNDAY && map.ContainsKey(d)) {
                        int[] ymd = { year, month, d };
                        int num = year <= 2006 ? 1 : 7;
                        for (int i = 1; i <= num; i++) {
                            ymd = Basic.GetTommorow(ymd[0], ymd[1], ymd[2]);
                            if (CheckHoliday(ymd[0], ymd[1], ymd[2]) == null) {
                                map.Add(ymd[2], NF);
                                break;
                            }
                        }
                    }
                }
            } else { /** ignore.(振替休日:1973-04から) **/}
            if (2003 <= year) {
                for (int d = 1; d <= last; d++) {
                    int dayOfWeek = (week + d - 1) % 7;
                    if (dayOfWeek != SUNDAY && !map.Keys.Contains(d)) {
                        int[] ymd = Basic.GetYesterday(year, month, d);
                        if (CheckHoliday(ymd[0], ymd[1], ymd[2]) != null) {
                            ymd = Basic.GetTommorow(year, month, d);
                            if (CheckHoliday(ymd[0], ymd[1], ymd[2]) != null)
                                map.Add(d, NK);
                        } else { /** not holiday. **/}
                    }
                }
            } else { /** ignore.(国民の休日:2003-01から) **/}
            return map;
        }

    }
}

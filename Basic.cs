
namespace Calendar {
    public static class Basic {
        public static int GetLastDay(int year, int month) {
            int last = 0;
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
                last = 31;
            } else if (month == 4 || month == 6 || month == 9 || month == 11) {
                last = 30;
            } else {
                last = 28 + (1 / (year - year / 4 * 4 + 1)) * (1 - 1 / (year - year / 100 * 100 + 1)) + (1 / (year - year / 400 * 400 + 1));
            }
            return last;
        }
        public static int GetFirstWeek(int year, int month) {
            if (month == 1 || month == 2) {
                year--;
                month += 12;
            }
            return (year + year / 4 - year / 100 + year / 400 + (13 * month + 8) / 5 + 1) % 7;
        }
        public static int[] GetTommorow(int year, int month, int day) {
            int[] ymd = { year, month, day };
            if (ymd[2] == GetLastDay(ymd[0], ymd[1])) {
                if (ymd[1] == 12) {
                    ymd[0]++;
                    ymd[1] = 1;
                } else {
                    ymd[1]++;
                }
                ymd[2] = 1;
            } else {
                ymd[2]++;
            }
            return ymd;
        }
        public static int[] GetYesterday(int year, int month, int day) {
            int[] ymd = { year, month, day };
            if (ymd[2] == 1) {
                if (ymd[1] == 1) {
                    ymd[0]--;
                    ymd[1] = 12;
                } else {
                    ymd[1]--;
                }
                ymd[2] = GetLastDay(ymd[0], ymd[0]);
            } else {
                ymd[2]--;
            }
            return ymd;
        }
    }
}

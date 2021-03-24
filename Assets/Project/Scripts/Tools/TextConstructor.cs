//Author : WJ Choi
using System;
using System.Text;

/// <summary>
/// 금전 관련 숫자 표기를 위한 형식
/// <para>JustComma : 1234567 -> 1,234,567</para>
/// 1000단위로 콤마만 붙여줌
/// 
/// <para>Fixed4 : 1234567 -> 1,234K</para>
/// KMBT를 붙여서 최대 4자리숫자까지 노출
/// 
/// <para>Fixed3 : 100161234567 -> 100B</para>
/// KMBT를 붙여서 최대 3자리숫자까지 노출
/// 
/// <para>Fixed3P1 : 1234567 -> 1.2M</para>
/// KMBT를 붙여서 소숫점이하 1자리까지 표시
/// 
/// <para>Conditional3P2 : 1000000 -> 1M</para>
/// 소숫점이하가 0일경우 표시안함. 0이아닐경우 최대 2자리까지
/// 
/// <para>Fixed6 : 123456789 -> 123,456K</para>
/// KMBT를 붙여서 최대 6자리숫자까지 노출
/// 
/// /// <para>Conditional3P1 : 1000000 -> 1M</para>
/// 소숫점이하가 0일경우 표시안함. 0이아닐경우 최대 1자리까지
/// </summary>
public enum MoneyFormat {
    JustComma,
    Fixed4, 
    Fixed3, 
    Fixed3P1, 
    Conditional3P2,
    Fixed15,
    Fixed9,
    Fixed6,
    Conditional3P1,
}
/// <summary>
/// 시간 관련 숫자 표기를 위한 형식
/// <para>WDHMS : 주일시분초 (1:1:23:45:00)</para>
/// <para>DHMS : 일시분초 (1:23:45:00)</para>
/// <para>HMS : 시분초 (123:45:00)</para>
/// <para>MS : 분초 (1:23)</para>
/// <para>Optional_WDHMS : 조건부 주일시분초. 주일시분이 0일경우에 "00:"등을 표현하지 않음.</para>
/// </summary>
public enum TimeFormat { WDHMS, DHMS, HMS, MS, Optional_WDHMS }

public enum FileSizeFormat { Auto, Fixed_KB, Fixed_MB }

public static class TextConstructor {
    public static string ToString(this int target, MoneyFormat format) {
        return StringBuild(target.ToString(), format);
    }

    public static string ToString(this long target, MoneyFormat format) {
        return StringBuild(target.ToString(), format);
    }

    private static string StringBuild(string numericString, MoneyFormat format) {
        switch (format) {
            case MoneyFormat.JustComma: {

                    int commaCount = (numericString.Length - 1) / 3;
                    int finalLength = numericString.Length + commaCount;
                    StringBuilder sb = new StringBuilder(numericString, finalLength);
                    for (int i = (numericString.Length + 2) % 3 + 1; i < finalLength; i += 4) {
                        sb.Insert(i, ',');
                    }
                    return sb.ToString();
                }

            case MoneyFormat.Fixed3: {

                    int significantLength = (numericString.Length + 2) % 3 + 1;
                    StringBuilder sb = new StringBuilder(numericString, 0, significantLength,
                        significantLength + (numericString.Length > 3 ? 1 : 0));  //KMBT태그 길이
                    if (numericString.Length > 3) {
                        sb.Append(KMBT_Tag(numericString.Length));
                    }
                    return sb.ToString();
                }

            case MoneyFormat.Fixed4: {

                    int significantLength = numericString.Length == 1 ? 1 : (numericString.Length + 1) % 3 + 2;
                    StringBuilder sb = new StringBuilder(numericString, 0, significantLength,
                        significantLength + (significantLength > 3 ? 1 : 0) + (numericString.Length > 4 ? 1 : 0));  //콤마(1) + KMBT태그(1) 길이

                    if (significantLength > 3) {
                        sb.Insert((significantLength + 2) % 3 + 1, ',');
                    }

                    if (numericString.Length > 4) {
                        sb.Append(KMBT_Tag(numericString.Length - 1));
                    }
                    return sb.ToString();
                }

            case MoneyFormat.Fixed3P1: {

                    int significantLength = (numericString.Length + 2) % 3 + 1; //소숫점 이상만
                    StringBuilder sb = new StringBuilder(numericString, 0, significantLength + (numericString.Length > 3 ? 1 : 0),
                        significantLength + (numericString.Length > 3 ? 3 : 0));  //소숫점이하(2) + KMBT태그(1) 길이
                    if (numericString.Length > 3) {
                        sb.Insert(significantLength, '.');  //소숫점 이하 표시
                        sb.Append(KMBT_Tag(numericString.Length));
                    }
                    return sb.ToString();
                }

            case MoneyFormat.Conditional3P1: {

                    int significantLength = (numericString.Length + 2) % 3 + 1; //소숫점 이상만
                    StringBuilder sb = new StringBuilder(numericString, 0, significantLength + (numericString.Length > 3 ? 1 : 0),
                        significantLength + (numericString.Length > 3 ? 3 : 0));  //소숫점이하(2) + KMBT태그(1) 길이
                    if (numericString.Length > 3) {
                        if (numericString[significantLength] != '0') {  //  소숫점이하 1번째자리는 0이아님
                            sb.Insert(significantLength, '.');          // 
                        } else {                                        //  소숫점이하 1번째자리도 0임
                            sb.Remove(significantLength, 1);            //  ---> 소숫점 표시 안함
                        }
                        sb.Append(KMBT_Tag(numericString.Length));
                    }
                    return sb.ToString();
                }

            case MoneyFormat.Conditional3P2: {

                    int significantLength = (numericString.Length + 2) % 3 + 1; //소숫점 이상만
                    StringBuilder sb = new StringBuilder(numericString, 0, significantLength + (numericString.Length > 3 ? 2 : 0),
                        significantLength + (numericString.Length > 3 ? 4 : 0));  //소숫점이하(3) + KMBT태그(1) 길이

                    if (numericString.Length > 3) { //1000이상
                        if (numericString[significantLength + 1] != '0') {  //소숫점 이하 2번째자리가 0이아님
                            sb.Insert(significantLength, '.');              //---> 소숫점 이하 2자리 다 표시
                        } else {                                            //소숫점이하 2번째자리가 0임
                            if (numericString[significantLength] != '0') {  //  소숫점이하 1번째자리는 0이아님
                                sb.Remove(significantLength + 1, 1);        //  ---> 소숫점 이하 1자리만 표시
                                sb.Insert(significantLength, '.');          //
                            } else {                                        //  소숫점이하 1번째자리도 0임
                                sb.Remove(significantLength, 2);            //  ---> 소숫점 표시 안함
                            }
                        }

                        sb.Append(KMBT_Tag(numericString.Length));
                    }
                    return sb.ToString();
                }

            case MoneyFormat.Fixed15: {

                    int significantLength = numericString.Length < 16 ? numericString.Length : ((numericString.Length - 1) % 3) + 13;
                    int commaCount = (significantLength - 1) / 3;
                    int finalLength = significantLength + commaCount;// + (numericString.Length > 15 ? 1 : 0);

                    StringBuilder sb = new StringBuilder(numericString, 0, significantLength, finalLength);  //콤마(1) + KMBT태그(1) 길이

                    // 3자리마다 콤마 찍어줌
                    for (int i = (significantLength + 2) % 3 + 1; i < finalLength; i += 4) {
                        sb.Insert(i, ',');
                    }

                    // 맨끝에 KMBT 태그붙임
                    if (numericString.Length > 15) {
                        sb.Append(KMBT_Tag(numericString.Length - 12));
                    }

                    return sb.ToString();
                }

            case MoneyFormat.Fixed9: {

                    int significantLength = numericString.Length < 10 ? numericString.Length : ((numericString.Length - 1) % 3) + 7;
                    int commaCount = (significantLength - 1) / 3;
                    int finalLength = significantLength + commaCount;

                    StringBuilder sb = new StringBuilder(numericString, 0, significantLength, finalLength);  //콤마(1) + KMBT태그(1) 길이

                    // 3자리마다 콤마 찍어줌
                    for (int i = (significantLength + 2) % 3 + 1; i < finalLength; i += 4) {
                        sb.Insert(i, ',');
                    }

                    // 맨끝에 KMBT 태그붙임
                    if (numericString.Length > 9) {
                        sb.Append(KMBT_Tag(numericString.Length - 6));
                    }

                    return sb.ToString();
                }

            case MoneyFormat.Fixed6: {

                    int significantLength = numericString.Length < 7 ? numericString.Length : ((numericString.Length - 1) % 3) + 4;
                    int commaCount = (significantLength - 1) / 3;
                    int finalLength = significantLength + commaCount;

                    StringBuilder sb = new StringBuilder(numericString, 0, significantLength, finalLength);  //콤마(1) + KMBT태그(1) 길이

                    // 3자리마다 콤마 찍어줌
                    for (int i = (significantLength + 2) % 3 + 1; i < finalLength; i += 4) {
                        sb.Insert(i, ',');
                    }

                    // 맨끝에 KMBT 태그붙임
                    if (numericString.Length > 6) {
                        sb.Append(KMBT_Tag(numericString.Length - 3));
                    }

                    return sb.ToString();
                }
        }
        return "";
    }

    private static char KMBT_Tag(int numberLength) {
        if (numberLength <= 3)
            return ' ';
        else if (numberLength <= 6)
            return 'K';
        else if (numberLength <= 9)
            return 'M';
        else if (numberLength <= 12)
            return 'B';
        else if (numberLength <= 15)
            return 'T';
        else
            return '?';
    }


    public static string ToString(this TimeSpan target, TimeFormat format) {
        int weeks = 0;
        int days = 0;
        int hours = 0;
        int minutes = 0;
        int seconds = 0;

        if (target.TotalSeconds < 0) {
            return new TimeSpan().ToString(format);
        }

        switch (format) {
            case TimeFormat.WDHMS:
                if (target.Milliseconds > 500) {
                    target.Add(TimeSpan.FromMilliseconds(500));
                }
                weeks = target.Days / 7;
                days = target.Days % 7;
                hours = target.Hours;
                minutes = target.Minutes;
                seconds = target.Seconds;
                return weeks + ":" + days + ":" + hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");

            case TimeFormat.DHMS:
                if (target.Milliseconds > 500) {
                    target.Add(TimeSpan.FromMilliseconds(500));
                }
                days = target.Days;
                hours = target.Hours;
                minutes = target.Minutes;
                seconds = target.Seconds;
                return days + ":" + hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");

            case TimeFormat.HMS:
                if (target.Milliseconds > 500) {
                    target.Add(TimeSpan.FromMilliseconds(500));
                }
                hours = (int)target.TotalHours;
                minutes = target.Minutes;
                seconds = target.Seconds;
                return hours + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");

            case TimeFormat.MS:
                if (target.Milliseconds > 500) {
                    target.Add(TimeSpan.FromMilliseconds(500));
                }
                minutes = (int)target.TotalMinutes;
                seconds = target.Seconds;
                return minutes + ":" + seconds.ToString("D2");

            case TimeFormat.Optional_WDHMS:
                weeks = target.Days / 7;
                if (weeks > 0) { return target.ToString(TimeFormat.WDHMS); }
                days = target.Days % 7;
                if (days > 0) { return target.ToString(TimeFormat.DHMS); }
                hours = target.Hours;
                if (hours > 0) { return target.ToString(TimeFormat.HMS); }
                return target.ToString(TimeFormat.MS);
        }

        return "";
    }

    public static string ToString(this long target, FileSizeFormat format) {
        double targetDouble = target;
        switch (format) {
            case FileSizeFormat.Auto:
                int TagIndex = 0;   //0 : 바이트, 1 : 키로바이트, 2 : 메가바이트..
                while (targetDouble > 1024) {
                    TagIndex++;
                    targetDouble /= 1024.0;
                }
                return string.Format("{0}{1}", targetDouble.ToString("N1"), FileSize_Tag(TagIndex));

            case FileSizeFormat.Fixed_KB:
                targetDouble /= 1024.0;
                return string.Format("{0}{1}", targetDouble.ToString("N1"), FileSize_Tag(1));

            case FileSizeFormat.Fixed_MB:
                targetDouble /= 1024.0;
                targetDouble /= 1024.0;
                return string.Format("{0}{1}", targetDouble.ToString("N1"), FileSize_Tag(2));

            default:
                return "?";
        }
    }
    
    private static string FileSize_Tag(int tagIndex) {
        switch (tagIndex) {
            case 0:
                return "Byte";
            case 1:
                return "KB";
            case 2:
                return "MB";
            case 3:
                return "GB";
            case 4:
                return "TB";
            default:
                return "?";
        }
    }
}

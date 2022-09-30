﻿namespace Hoyo.Extensions;
/// <summary>
/// DateTime扩展
/// </summary>
public static class DateTimeExtension
{
    /// <summary>
    /// 获取整周的星期数字形式
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    private static int DayNumber(this DayOfWeek day) => day switch
    {
        DayOfWeek.Friday => 5,
        DayOfWeek.Monday => 1,
        DayOfWeek.Saturday => 6,
        DayOfWeek.Thursday => 4,
        DayOfWeek.Tuesday => 2,
        DayOfWeek.Wednesday => 3,
        _ => 7,
    };
    /// <summary>
    /// 获取某天开始时间
    /// </summary>
    /// <param name="dateTime">某天中的任意时间</param>
    /// <returns></returns>
    public static DateTime DayStart(this DateTime dateTime) => dateTime.Date;
    /// <summary>
    /// 获取某天结束时间
    /// </summary>
    /// <param name="dateTime">某天中的任意时间</param>
    /// <returns></returns>
    public static DateTime DayEnd(this DateTime dateTime) => dateTime.DayStart().AddDays(1).AddMilliseconds(-1);
    /// <summary>
    /// 获取某天的始末时间
    /// </summary>
    /// <param name="dateTime">某天中的任意时间</param>
    /// <returns>(Start, End)</returns>
    public static ValueTuple<DateTime, DateTime> DayStartEnd(this DateTime dateTime) => new(dateTime.DayStart(), dateTime.DayEnd());
    /// <summary>
    /// 获取某天的所属周的开始时间
    /// </summary>
    /// <param name="dateTime">某周中的任意天日期</param>
    /// <param name="firstDay">一周的第一天[周日还是周一或者其他]</param>
    /// <returns></returns>
    public static DateTime WeekStart(this DateTime dateTime, DayOfWeek firstDay) => dateTime.AddDays(-dateTime.DayOfWeek.DayNumber()).DayStart().AddDays((int)firstDay);

    /// <summary>
    /// 获取某天的所属周的结束时间
    /// </summary>
    /// <param name="dateTime">某周中的任意天日期</param>
    /// <param name="firstDay">一周的第一天[周日还是周一或者其他]</param>
    /// <returns></returns>
    public static DateTime WeekEnd(this DateTime dateTime, DayOfWeek firstDay) => dateTime.WeekStart(firstDay).AddDays(6).DayEnd();
    /// <summary>
    /// 获取某天所属周的开始和结束时间
    /// </summary>
    /// <param name="dateTime">某周中的任意天日期</param>
    /// <param name="firstDay">一周的第一天[周日还是周一或者其他]</param>
    /// <returns></returns>
    public static ValueTuple<DateTime, DateTime> WeekStartEnd(this DateTime dateTime, DayOfWeek firstDay) => new(dateTime.WeekStart(firstDay), dateTime.WeekEnd(firstDay));

    /// <summary>
    /// 获取某月的开始时间
    /// </summary>
    /// <param name="dateTime">某月中的任意天日期</param>
    /// <returns></returns>
    public static DateTime MonthStart(this DateTime dateTime) => dateTime.DayStart().AddDays(1 - dateTime.Day);
    /// <summary>
    /// 获取某月的结束时间
    /// </summary>
    /// <param name="dateTime">某月中的任意天日期</param>
    /// <returns></returns>
    public static DateTime MonthEnd(this DateTime dateTime) => dateTime.MonthStart().AddMonths(1).AddMilliseconds(-1);
    /// <summary>
    /// 获取某月的始末时间
    /// </summary>
    /// <param name="dateTime">某天中的任意时间</param>
    /// <returns>(Start, End)</returns>
    public static ValueTuple<DateTime, DateTime> MonthStartEnd(this DateTime dateTime) => new(dateTime.MonthStart(), dateTime.MonthEnd());
    /// <summary>
    /// 获取某年的开始时间
    /// </summary>
    /// <param name="dateTime">某年中的任意一天</param>
    /// <returns></returns>
    public static DateTime YearStart(this DateTime dateTime) => dateTime.Date.AddMonths(1 - dateTime.Month).AddDays(1 - dateTime.Day).DayStart();
    /// <summary>
    /// 获取某年的结束时间
    /// </summary>
    /// <param name="dateTime">某年中的任意一天</param>
    /// <returns></returns>
    public static DateTime YearEnd(this DateTime dateTime) => dateTime.YearStart().AddYears(1).AddMilliseconds(-1);
    /// <summary>
    /// 获取某年的始末时间
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>(Start, End)</returns>
    public static ValueTuple<DateTime, DateTime> YearStartEnd(this DateTime dateTime) => new(dateTime.YearStart(), dateTime.YearEnd());
    /// <summary>
    /// 根据周数和年份获取某周的开始和结束时间
    /// </summary>
    /// <param name="week">一年中自然周数</param>
    /// <param name="year">年份</param>
    /// <param name="firstDay">一周开始时间(周一或者周日)</param>
    /// <returns></returns>
    public static ValueTuple<DateTime, DateTime> WeekStartEndByNumber(this int week, int year, DayOfWeek firstDay) => new DateTime(year, 1, 1).AddDays((week - 1) * 7).WeekStartEnd(firstDay);
    /// <summary>
    /// 根据月份获取某月的开始时间和结束时间
    /// </summary>
    /// <param name="month">月份</param>
    /// <param name="year">年份</param>
    /// <returns></returns>
    public static ValueTuple<DateTime, DateTime> MonthStartEndByMonth(this int month, int year) => month < 1 | month > 13 ? throw new("非法月份") : new DateTime(year, month, 2).MonthStartEnd();
}
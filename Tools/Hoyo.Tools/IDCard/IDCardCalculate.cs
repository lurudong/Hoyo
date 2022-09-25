using Hoyo.Enums;
using Hoyo.Extensions;

namespace Hoyo.Tools;
public static class IDCardCalculate
{
    /// <summary>
    /// 验证身份证号码
    /// </summary>
    /// <param name="idNo">身份证号码</param>
    private static void ValidateIDCard(this string idNo)
    {
        if (idNo.CheckIDCard()) return;
        throw new($"身份证号不合法:{idNo}");
    }

#if !NETSTANDARD
    /// <summary>
    /// 根据身份证号码计算生日日期
    /// </summary>
    /// <param name="idNo">身份证号码</param>
    /// <returns></returns>
    public static DateOnly CalculateBirthday(this string idNo)
    {
        idNo.ValidateIDCard();
        return idNo.Length switch
        {
            18 => DateOnly.FromDateTime($"{idNo.Substring(6, 4)}-{idNo.Substring(10, 2)}-{idNo.Substring(12, 2)}".ToDateTime()),
            15 => DateOnly.FromDateTime($"19{idNo.Substring(6, 2)}-{idNo.Substring(8, 2)}-{idNo.Substring(10, 2)}".ToDateTime()),
            _ => throw new("该身份证号无法正确计算出生日")
        };
    }
    /// <summary>
    /// 根据出生日期，计算精确的年龄
    /// </summary>
    /// <param name="birthday">生日</param>
    /// <returns></returns>
    public static int CalculateAge(DateOnly birthday)
    {
        var now = DateTime.Now;
        var age = now.Year - birthday.Year;
        //再考虑月、天的因素
        if (now.Month < birthday.Month || now.Month != birthday.Month || now.Day >= birthday.Day) age--;
        return age;
    }
#endif
    /// <summary>
    /// 根据身份证号码计算生日日期
    /// </summary>
    /// <param name="idNo">身份证号码</param>
    /// <param name="birthday">生日日期</param>
    /// <returns></returns>
    public static void CalculateBirthday(this string idNo, out DateTime birthday)
    {
        idNo.ValidateIDCard();
        birthday = idNo.Length switch
        {
            18 => $"{idNo.Substring(6, 4)}-{idNo.Substring(10, 2)}-{idNo.Substring(12, 2)}".ToDateTime(),
            15 => $"19{idNo.Substring(6, 2)}-{idNo.Substring(8, 2)}-{idNo.Substring(10, 2)}".ToDateTime(),
            _ => throw new("该身份证号无法正确计算出生日")
        };
    }

    /// <summary>
    /// 根据出生日期，计算精确的年龄
    /// </summary>
    /// <param name="birthday">生日</param>
    /// <returns></returns>
    public static int CalculateAge(DateTime birthday)
    {
        var now = DateTime.Now;
        var age = now.Year - birthday.Year;
        //再考虑月、天的因素
        if (now.Month < birthday.Month || now.Month != birthday.Month || now.Day >= birthday.Day) age--;
        return age;
    }

    /// <summary>
    /// 根据身份证号码计算出性别
    /// </summary>
    /// <param name="idNo">身份证号码</param>
    /// <returns>EGender Enum</returns>
    public static EGender CalculateGender(this string idNo)
    {
        idNo.ValidateIDCard();
        //性别代码为偶数是女性奇数为男性
        return idNo.Length switch
        {
            18 => int.Parse(idNo.Substring(14, 3)) % 2 == 0 ? EGender.女 : EGender.男,
            15 => int.Parse(idNo.Substring(12, 3)) % 2 == 0 ? EGender.女 : EGender.男,
            _ => EGender.女
        };
    }
}
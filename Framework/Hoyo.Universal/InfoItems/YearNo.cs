namespace Hoyo.Universal;
public class YearNoStr : YearNo
{
    /// <summary>
    /// 存储源值(常用于导入)
    /// </summary>
    public string Str { get; set; } = string.Empty;
    public bool Fill()
    {
        if (string.IsNullOrWhiteSpace(Str)) return false;
        if (Str.Length == 6)
        {
            if (int.TryParse(Str, out var yn) == false) return false;
            Year = yn / 100;
            No = yn % 100;
            return true;
        }
        var strs = Str.Split('/', '-', '_', ' ', '|');
        if (strs.Length != 2) return false;
        if (int.TryParse(strs[0], out var y) == false) return false;
        if (int.TryParse(strs[1], out var n) == false) return false;
        Year = y;
        No = n;
        return true;
    }
    public YearNo GetYearNo() => new() { Year = Year, No = No };

}
public class YearNo : IIntegrate
{
    public YearNo() { }
    public YearNo(int year, int no) { Year = year; No = no; }
    public int Year { get; set; }
    public int No { get; set; }

    public override string ToString() => $"{Year}{No.ToString().PadLeft(2, '0')}";
    public bool IsIntegrated() => Year != 0 && No != 0;

    public static YearNo? Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        if (value.Length == 6)
        {
            return int.TryParse(value, out var yn) == false ? null : new() { Year = yn / 100, No = yn % 100 };
        }
        var strs = value.Split('/', '-', '_', ' ', '|');
        return strs.Length != 2
            ? null
            : int.TryParse(strs[0], out var y) == false
            ? null
            : int.TryParse(strs[1], out var n) == false ? null : new() { Year = y, No = n };
    }
}
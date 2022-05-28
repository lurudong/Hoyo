namespace Hoyo.Universal;

public class YMNumber : IFill
{
    public YMNumber() { }
    public YMNumber(int y, int m) { Y = y; M = m; }
    public int? Y { get; set; }
    public int? M { get; set; }
    public string Str { get; set; } = string.Empty;
    public string? Format { get; set; }
    public string? FormatStr => Y != null ? Y.ToString() + (Format ?? "/") + (M == null ? null : M < 10 ? "0" + M.ToString() : M.ToString()) : null;
    public override string? ToString() => M is null ? Convert.ToString(Y) : Y is null ? Convert.ToString(M) : ((Y * 100) + M).ToString();
    public string? Fill(object sourceValue)
    {
        Str = sourceValue.ToString()!;
        return FillByStr();
    }

    public static YMNumber? Parse(string value, string? format = null)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        switch (value.Length)
        {
            case 6 when int.TryParse(value, out var date):
                YMNumber ym = new()
                {
                    Y = date / 100,
                    M = date % 100,
                    Format = format
                };
                ym.Str = ((ym.Y * 100) + ym.M).ToString()!;
                return ym;
            case 4 when int.TryParse(value, out var year):
                return new()
                {
                    Y = year,
                    Format = format,
                    Str = value
                };
            default:
                return null;
        }
    }

    public string? FillByStr()
    {
        if (string.IsNullOrWhiteSpace(Str)) return "值不能为空";
        if (DateTime.TryParse(Str, out var dtValue))
        {
            Y = dtValue.Year;
            M = dtValue.Month;
            return null;
        }
        switch (Str.Length)
        {
            case 8 when int.TryParse(Str, out var intValue):
                Y = intValue / 10000;
                M = intValue % 10000 / 100;
                return null;
            case 6:
                {
                    if (!int.TryParse(Str, out var date)) return null;
                    Y = date / 100;
                    M = date % 100;
                    Str = ((Y * 100) + M).ToString()!;
                    return null;
                }
            case 4:
                {
                    if (int.TryParse(Str, out var year))
                    {
                        Y = year;
                    }
                    return null;
                }
            default:
                {
                    Str = Str.Replace("&", "-").Replace(" ", "-").Replace(".", "-").Replace(",", "-").Replace("/", "-").Replace("\\", "-").Replace("|", "-");
                    if (!DateTime.TryParse(Str, out var date)) return null;
                    Y = date.Year;
                    M = date.Month;
                    Str = ((Y * 100) + M).ToString()!;
                    return null;
                }
        }
    }
}
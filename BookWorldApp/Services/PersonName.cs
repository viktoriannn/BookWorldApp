namespace BookWorldApp.Services;

public static class PersonName
{
    public static string Short(string surname, string name, string? patronymic)
    {
        var initials = string.Concat(new[] { name, patronymic }
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .Select(part => $"{part![0]}. "));
        return $"{surname} {initials}".Trim();
    }

    public static string Full(string surname, string name, string? patronymic)
        => string.Join(' ', new[] { surname, name, patronymic }.Where(part => !string.IsNullOrWhiteSpace(part)));
}

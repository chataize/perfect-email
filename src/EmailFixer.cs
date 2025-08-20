namespace ChatAIze.PerfectEmail;

public static class EmailTypos
{
    private const string CanonGmail = "gmail.com";

    private const string CanonHotmail = "hotmail.com";

    private const string CanonIcloud = "icloud.com";

    private const string CanonOutlook = "outlook.com";

    private const string CanonYahoo = "yahoo.com";

    private static readonly HashSet<string> GmailTypos = new(StringComparer.Ordinal)
    {
        "gamial.com", "gamil.co", "gamil.com", "gemail.com", "gimail.com", "gma.com", "gmaal.com", "gmai.com",
        "gmaii.com", "gmaiil.com", "gmaik.com", "gmail.cim", "gmail.cm", "gmail.co", "gmail.comm", "gmail.con",
        "gmail.cpm", "gmail.dom", "gmail.om", "gmail.vom", "gmail.xom", "gmailc.om", "gmailk.com", "gmaill.com",
        "gmailm.com", "gmailn.com", "gmaio.com", "gmaip.com", "gmal.com", "gmali.com", "gmaul.com", "gmial.com",
        "gmil.com", "gmqil.com", "gnail.com", "gogglemail.com", "googlemail.com"
    };

    private static readonly HashSet<string> HotmailTypos = new(StringComparer.Ordinal)
    {
        "homail.com", "hootmail.com", "hormail.com", "hotmai.com", "hotmaik.com", "hotmail.cim", "hotmail.cm",
        "hotmail.co", "hotmail.comm", "hotmail.con", "hotmail.cpm", "hotmail.om", "hotmaill.com", "hotmal.com",
        "hotmale.com", "hotmali.com", "hotmial.co", "hotmial.com", "hotmil.com", "hotnail.com"
    };

    private static readonly HashSet<string> IcloudTypos = new(StringComparer.Ordinal)
    {
        "icload.com", "iclod.cim", "iclod.co", "iclod.com", "iclodmail.com", "iclou.com", "icloud.cim", "icloud.cm",
        "icloud.co", "icloud.comm", "icloud.con", "icloud.cpm", "icloud.om", "icloudd.com", "icloude.com", "iclould.com",
        "iclud.co", "iclud.com", "icluod.com", "icoud.com"
    };

    private static readonly HashSet<string> OutlookTypos = new(StringComparer.Ordinal)
    {
        "otlook.com", "otulook.com", "oulok.com", "oultoook.com", "outllk.com", "outllok.com", "outllook.com",
        "outlluk.com", "outlock.com", "outlok.co", "outlok.com", "outlokk.com", "outlook.cim", "outlook.cm",
        "outlook.co", "outlook.comm", "outlook.con", "outlook.cpm", "outlook.om", "outloook.com", "outluk.com",
        "outlukc.com", "outolok.com"
    };

    private static readonly HashSet<string> YahooTypos = new(StringComparer.Ordinal)
    {
        "yaahu.com", "yahho.com", "yaho.co", "yaho.com", "yaho0.com", "yahoo.cim", "yahoo.cm", "yahoo.co",
        "yahoo.comm", "yahoo.con", "yahoo.cpm", "yahoo.om", "yahooh.com", "yahool.com", "yahu.com", "yaoho.com",
        "yaohoo.com", "yaoo.com", "yhaoo.com", "yhoo.com"
    };

    public static string FixTypos(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        if (!EmailValidator.IsValidEmail(email))
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }

        email = email.Trim().ToLowerInvariant();

        var at = email.LastIndexOf('@');
        if (at <= 0 || at == email.Length - 1)
        {
            return email;
        }

        var domain = email.AsSpan(at + 1);
        if (domain.SequenceEqual(CanonGmail.AsSpan()) ||
            domain.SequenceEqual(CanonHotmail.AsSpan()) ||
            domain.SequenceEqual(CanonIcloud.AsSpan()) ||
            domain.SequenceEqual(CanonOutlook.AsSpan()) ||
            domain.SequenceEqual(CanonYahoo.AsSpan()))
        {
            return email;
        }

        var corrected = TryExact(domain) ?? TryFuzzySafely(domain);
        return corrected is null ? email : string.Concat(email.AsSpan(0, at), "@", corrected);
    }

    private static string? TryExact(ReadOnlySpan<char> domain)
    {
        var domainString = domain.ToString();

        if (GmailTypos.Contains(domainString))
        {
            return CanonGmail;
        }

        if (HotmailTypos.Contains(domainString))
        {
            return CanonHotmail;
        }

        if (IcloudTypos.Contains(domainString))
        {
            return CanonIcloud;
        }

        if (OutlookTypos.Contains(domainString))
        {
            return CanonOutlook;
        }

        if (YahooTypos.Contains(domainString))
        {
            return CanonYahoo;
        }

        return null;
    }

    private static string? TryFuzzySafely(ReadOnlySpan<char> domain)
    {
        if (domain.IsEmpty) return null;

        switch (domain[0])
        {
            case 'g':
                if (IsDamerauLeq1(domain, CanonGmail.AsSpan()))
                {
                    return CanonGmail;
                }
                break;
            case 'h':
                if (IsDamerauLeq1(domain, CanonHotmail.AsSpan()))
                {
                    return CanonHotmail;
                }
                break;
            case 'i':
                if (IsDamerauLeq1(domain, CanonIcloud.AsSpan()))
                {
                    return CanonIcloud;
                }
                break;
            case 'o':
                if (IsDamerauLeq1(domain, CanonOutlook.AsSpan()))
                {
                    return CanonOutlook;
                }
                break;
            case 'y':
                if (IsDamerauLeq1(domain, CanonYahoo.AsSpan()))
                {
                    return CanonYahoo;
                }
                break;
        }

        return null;
    }

    private static bool IsDamerauLeq1(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        if (a.SequenceEqual(b))
        {
            return true;
        }

        var na = a.Length;
        var nb = b.Length;
        var diff = na - nb;

        if (diff < -1 || diff > 1)
        {
            return false;
        }

        if (diff == 0)
        {
            var first = -1;
            var second = -1;

            for (var i = 0; i < na; i++)
            {
                if (a[i] == b[i])
                {
                    continue;
                }

                if (first < 0) first = i;
                else
                {
                    second = i;
                    break;
                }
            }

            if (first < 0)
            {
                return true;
            }

            if (second < 0)
            {
                return true;
            }

            return second == first + 1 && a[first] == b[second] && a[second] == b[first];
        }

        var i2 = 0;
        var j = 0;
        var edits = 0;

        while (i2 < na && j < nb)
        {
            if (a[i2] == b[j])
            {
                i2++; j++;
                continue;
            }

            if (++edits > 1)
            {
                return false;
            }

            if (na > nb)
            {
                i2++;
            }
            else
            {
                j++;
            }
        }

        return true;
    }
}

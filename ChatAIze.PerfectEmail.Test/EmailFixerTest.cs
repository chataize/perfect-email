namespace ChatAIze.PerfectEmail.Test;

[TestClass]
public class EmailFixerTest
{
    [TestMethod]
    public void Canonical_Gmail_ReturnsSame()
    {
        AssertFix("user@gmail.com", "user@gmail.com");
    }

    [TestMethod]
    public void Canonical_Others_ReturnSame()
    {
        AssertFix("a@hotmail.com", "a@hotmail.com");
        AssertFix("b@icloud.com", "b@icloud.com");
        AssertFix("c@outlook.com", "c@outlook.com");
        AssertFix("d@yahoo.com", "d@yahoo.com");
    }

    [TestMethod]
    public void Normalization_TrimAndLowercase_Applied()
    {
        AssertFix("  User.Name+tag@GmAiL.CoM  ", "user.name+tag@gmail.com");
    }

    [TestMethod]
    public void Exact_Gmail_Typos_Fixed()
    {
        AssertFix("user@gmai.com", "user@gmail.com");
        AssertFix("user@gmal.com", "user@gmail.com");
        AssertFix("user@gnail.com", "user@gmail.com");
        AssertFix("user@gmaik.com", "user@gmail.com");
        AssertFix("user@gmaiil.com", "user@gmail.com");
        AssertFix("user@gmaill.com", "user@gmail.com");
        AssertFix("user@gmail.cm", "user@gmail.com");
        AssertFix("user@gmail.cim", "user@gmail.com");
        AssertFix("user@gmail.con", "user@gmail.com");
        AssertFix("user@gmail.comm", "user@gmail.com");
        AssertFix("user@gmail.cpm", "user@gmail.com");
        AssertFix("user@gmail.om", "user@gmail.com");
        AssertFix("user@gmail.vom", "user@gmail.com");
        AssertFix("user@gmail.xom", "user@gmail.com");
        AssertFix("user@gmailc.om", "user@gmail.com");
        AssertFix("user@gmqil.com", "user@gmail.com");
        AssertFix("user@gimail.com", "user@gmail.com");
        AssertFix("user@gemail.com", "user@gmail.com");
        AssertFix("user@gogglemail.com", "user@gmail.com");
        AssertFix("user@googlemail.com", "user@gmail.com");
    }

    [TestMethod]
    public void Exact_Hotmail_Typos_Fixed()
    {
        AssertFix("user@hotmial.com", "user@hotmail.com");
        AssertFix("user@hotmai.com", "user@hotmail.com");
        AssertFix("user@hotmaik.com", "user@hotmail.com");
        AssertFix("user@hotmaill.com", "user@hotmail.com");
        AssertFix("user@hotmil.com", "user@hotmail.com");
        AssertFix("user@hotmal.com", "user@hotmail.com");
        AssertFix("user@hotmale.com", "user@hotmail.com");
        AssertFix("user@homail.com", "user@hotmail.com");
        AssertFix("user@hootmail.com", "user@hotmail.com");
        AssertFix("user@hormail.com", "user@hotmail.com");
        AssertFix("user@hotmail.cm", "user@hotmail.com");
        AssertFix("user@hotmail.cim", "user@hotmail.com");
        AssertFix("user@hotmail.con", "user@hotmail.com");
        AssertFix("user@hotmail.comm", "user@hotmail.com");
        AssertFix("user@hotmail.cpm", "user@hotmail.com");
        AssertFix("user@hotmail.om", "user@hotmail.com");
    }

    [TestMethod]
    public void Exact_Icloud_Typos_Fixed()
    {
        AssertFix("user@iclud.com", "user@icloud.com");
        AssertFix("user@iclud.co", "user@icloud.com");
        AssertFix("user@icluod.com", "user@icloud.com");
        AssertFix("user@iclou.com", "user@icloud.com");
        AssertFix("user@icloude.com", "user@icloud.com");
        AssertFix("user@iclould.com", "user@icloud.com");
        AssertFix("user@icload.com", "user@icloud.com");
        AssertFix("user@iclod.com", "user@icloud.com");
        AssertFix("user@icloud.cm", "user@icloud.com");
        AssertFix("user@icloud.cim", "user@icloud.com");
        AssertFix("user@icloud.con", "user@icloud.com");
        AssertFix("user@icloud.comm", "user@icloud.com");
        AssertFix("user@icloud.cpm", "user@icloud.com");
        AssertFix("user@icloud.om", "user@icloud.com");
    }

    [TestMethod]
    public void Exact_Outlook_Typos_Fixed()
    {
        AssertFix("user@outlok.com", "user@outlook.com");
        AssertFix("user@outlokk.com", "user@outlook.com");
        AssertFix("user@outloook.com", "user@outlook.com");
        AssertFix("user@outllok.com", "user@outlook.com");
        AssertFix("user@outllook.com", "user@outlook.com");
        AssertFix("user@outlock.com", "user@outlook.com");
        AssertFix("user@otlook.com", "user@outlook.com");
        AssertFix("user@otulook.com", "user@outlook.com");
        AssertFix("user@outlukc.com", "user@outlook.com");
        AssertFix("user@outolok.com", "user@outlook.com");
        AssertFix("user@outlook.cm", "user@outlook.com");
        AssertFix("user@outlook.cim", "user@outlook.com");
        AssertFix("user@outlook.co", "user@outlook.com");
        AssertFix("user@outlook.con", "user@outlook.com");
        AssertFix("user@outlook.comm", "user@outlook.com");
        AssertFix("user@outlook.cpm", "user@outlook.com");
        AssertFix("user@outlook.om", "user@outlook.com");
        AssertFix("user@outlok.co", "user@outlook.com");
    }

    [TestMethod]
    public void Exact_Yahoo_Typos_Fixed()
    {
        AssertFix("user@yaho0.com", "user@yahoo.com");
        AssertFix("user@yahho.com", "user@yahoo.com");
        AssertFix("user@yaho.com", "user@yahoo.com");
        AssertFix("user@yaho.co", "user@yahoo.com");
        AssertFix("user@yahoo.cm", "user@yahoo.com");
        AssertFix("user@yahoo.cim", "user@yahoo.com");
        AssertFix("user@yahoo.con", "user@yahoo.com");
        AssertFix("user@yahoo.comm", "user@yahoo.com");
        AssertFix("user@yahoo.cpm", "user@yahoo.com");
        AssertFix("user@yahoo.om", "user@yahoo.com");
        AssertFix("user@yahool.com", "user@yahoo.com");
        AssertFix("user@yahooh.com", "user@yahoo.com");
        AssertFix("user@yhoo.com", "user@yahoo.com");
        AssertFix("user@yaoo.com", "user@yahoo.com");
        AssertFix("user@yhaoo.com", "user@yahoo.com");
        AssertFix("user@yaohoo.com", "user@yahoo.com");
    }

    [TestMethod]
    public void Fuzzy_Transpositions_Fixed()
    {
        AssertFix("user@gmail.cmo", "user@gmail.com");
        AssertFix("user@outlook.cmo", "user@outlook.com");
        AssertFix("user@icluod.com", "user@icloud.com");
    }

    [TestMethod]
    public void Fuzzy_SingleSubstitutions_Fixed()
    {
        AssertFix("user@gmail.xom", "user@gmail.com");
        AssertFix("user@outlouk.com", "user@outlook.com");
        AssertFix("user@yahol.com", "user@yahoo.com");
    }

    [TestMethod]
    public void Fuzzy_InsertionAndDeletion_Fixed()
    {
        AssertFix("user@outlok.com", "user@outlook.com");
        AssertFix("user@outloook.com", "user@outlook.com");
        AssertFix("user@gmail.coom", "user@gmail.com");
    }

    [TestMethod]
    public void Normalization_MoreWhitespace_Applied()
    {
        AssertFix("\tUser+tag@GMAIL.COM\n", "user+tag@gmail.com");
        AssertFix("\u00A0NAME@Yahoo.Com\u00A0", "name@yahoo.com");
    }

    [TestMethod]
    public void Idempotency_RunningTwice_NoFurtherChange()
    {
        var once = EmailFixer.FixEmail("  User.Name+Tag@GmAiL.CoM  ");
        var twice = EmailFixer.FixEmail(once);
        Assert.AreEqual(once, twice);

        once = EmailFixer.FixEmail("name@outlook.coom");
        twice = EmailFixer.FixEmail(once);
        Assert.AreEqual(once, twice);
    }

    [TestMethod]
    public void UnknownDomains_NoChange()
    {
        AssertFix("user@protonmail.com", "user@protonmail.com");
        AssertFix("user@example.com", "user@example.com");
        AssertFix("user@ymail.com", "user@ymail.com");
        AssertFix("user@mail.gmail.com", "user@mail.gmail.com");
        AssertFix("user@yahoo.co.uk", "user@yahoo.co.uk");
        AssertFix("user@outlook.co.uk", "user@outlook.co.uk");
        AssertFix("user@xmail.com", "user@xmail.com");
        AssertFix("user@gnail.co.uk", "user@gnail.co.uk");
    }

    [TestMethod]
    public void WhitespaceOnly_Throws()
    {
        Assert.ThrowsExactly<ArgumentException>(() => EmailFixer.FixEmail("   "));
    }

    [TestMethod]
    public void InvalidEmail_Throws()
    {
        Assert.ThrowsExactly<ArgumentException>(() => EmailFixer.FixEmail("not-an-email"));
        Assert.ThrowsExactly<ArgumentException>(() => EmailFixer.FixEmail("user@"));
        Assert.ThrowsExactly<ArgumentException>(() => EmailFixer.FixEmail("@domain.com"));
        Assert.ThrowsExactly<ArgumentException>(() => EmailFixer.FixEmail("user@@gmail.com"));
        Assert.ThrowsExactly<ArgumentException>(() => EmailFixer.FixEmail("user@ gmail.com"));
        Assert.ThrowsExactly<ArgumentException>(() => EmailFixer.FixEmail("user@gmail.com."));
        Assert.ThrowsExactly<ArgumentNullException>(() => EmailFixer.FixEmail(null!));
    }

    [TestMethod]
    public void ExactTypos_Various_Fixed()
    {
        AssertFix("user@homail.com", "user@hotmail.com");
        AssertFix("user@iclou.com", "user@icloud.com");
        AssertFix("user@outllok.com", "user@outlook.com");
        AssertFix("user@yahho.com", "user@yahoo.com");
        AssertFix("user@gmail.comm", "user@gmail.com");
    }

    [TestMethod]
    public void Exact_Typo_Gmail_Fixed()
    {
        AssertFix("user@gmai.com", "user@gmail.com");
        AssertFix("user@googlemail.com", "user@gmail.com");
        AssertFix("user@gmal.com", "user@gmail.com");
    }

    [TestMethod]
    public void Exact_Typo_Hotmail_Fixed()
    {
        AssertFix("user@hotmial.com", "user@hotmail.com");
    }

    [TestMethod]
    public void Exact_Typo_Icloud_Fixed()
    {
        AssertFix("user@iclud.com", "user@icloud.com");
    }

    [TestMethod]
    public void Exact_Typo_Outlook_Fixed()
    {
        AssertFix("user@outlok.com", "user@outlook.com");
    }

    [TestMethod]
    public void Exact_Typo_Yahoo_Fixed()
    {
        AssertFix("user@yaho0.com", "user@yahoo.com");
    }

    [TestMethod]
    public void Fuzzy_Transposition_Fixed_Originals()
    {
        AssertFix("user@gmail.cmo", "user@gmail.com");
        AssertFix("user@outlook.cmo", "user@outlook.com");
    }

    [TestMethod]
    public void Fuzzy_SingleSubstitution_Fixed_Originals()
    {
        AssertFix("user@gmail.xom", "user@gmail.com");
    }

    [TestMethod]
    public void Fuzzy_InsertionOrDeletion_Fixed_Originals()
    {
        AssertFix("user@outlok.com", "user@outlook.com");
        AssertFix("user@outloook.com", "user@outlook.com");
    }

    [TestMethod]
    public void UnknownDomain_NoChange_Originals()
    {
        AssertFix("user@protonmail.com", "user@protonmail.com");
        AssertFix("user@example.com", "user@example.com");
    }

    [TestMethod]
    public void Fuzzy_Guard_NoCrossBrandFalsePositive_Originals()
    {
        AssertFix("user@post.com", "user@post.com");
    }

    [TestMethod]
    public void Fuzzy_MoreThanOneEdit_NoChange_Originals()
    {
        AssertFix("user@gmaailx.com", "user@gmaailx.com");
    }

    [TestMethod]
    public void Idempotency_RunningTwice_DoesNotChangeResult_Original()
    {
        var input = "  User.Name+Tag@GmAiL.CoM  ";
        var once = EmailFixer.FixEmail(input);
        var twice = EmailFixer.FixEmail(once);

        Assert.AreEqual(once, twice);
    }

    private static void AssertFix(string input, string expected)
    {
        var actual = EmailFixer.FixEmail(input);
        Assert.AreEqual(expected, actual);
    }
}

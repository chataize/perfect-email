namespace ChatAIze.PerfectEmail.Test;

[TestClass]
public class EmailFixerTests
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
    public void Fuzzy_Transposition_Fixed()
    {
        AssertFix("user@gmail.cmo", "user@gmail.com");
        AssertFix("user@outlook.cmo", "user@outlook.com");
    }

    [TestMethod]
    public void Fuzzy_SingleSubstitution_Fixed()
    {
        AssertFix("user@gmail.xom", "user@gmail.com");
    }

    [TestMethod]
    public void Fuzzy_InsertionOrDeletion_Fixed()
    {
        AssertFix("user@outlok.com", "user@outlook.com");
        AssertFix("user@outloook.com", "user@outlook.com");
    }

    [TestMethod]
    public void UnknownDomain_NoChange()
    {
        AssertFix("user@protonmail.com", "user@protonmail.com");
        AssertFix("user@example.com", "user@example.com");
    }

    [TestMethod]
    public void Fuzzy_Guard_NoCrossBrandFalsePositive()
    {
        AssertFix("user@post.com", "user@post.com");
    }

    [TestMethod]
    public void Fuzzy_MoreThanOneEdit_NoChange()
    {
        AssertFix("user@gmaailx.com", "user@gmaailx.com");
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
    }

    [TestMethod]
    [DataRow("user@homail.com", "user@hotmail.com")]
    [DataRow("user@iclou.com", "user@icloud.com")]
    [DataRow("user@outllok.com", "user@outlook.com")]
    [DataRow("user@yahho.com", "user@yahoo.com")]
    [DataRow("user@gmail.comm", "user@gmail.com")]
    public void ExactTypos_Various_Fixed(string input, string expected)
    {
        AssertFix(input, expected);
    }

    private static void AssertFix(string input, string expected)
    {
        var actual = EmailFixer.FixEmail(input);
        Assert.AreEqual(expected, actual);
    }
}

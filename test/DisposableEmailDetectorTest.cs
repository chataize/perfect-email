namespace ChatAIze.PerfectEmail.Test;

[TestClass]
public sealed class DisposableEmailDetectorTest
{
    [DataTestMethod]
    [DataRow(null, false)]
    [DataRow("", false)]
    [DataRow("someone", false)]
    [DataRow("gmail.com", false)]
    [DataRow("@gmail.com", false)]
    [DataRow("  @gmail.com", false)]
    [DataRow("@gmail.com  ", false)]
    [DataRow("0-mail.com", true)]
    [DataRow("  0-mail.com", true)]
    [DataRow("0-mail.com  ", true)]
    [DataRow("@0-mail.com", true)]
    [DataRow("@0-maIl.Com", true)]
    [DataRow("someone@gmail.com", false)]
    [DataRow("someone@0-mail.com", true)]
    public void IsDisposableEmailTest(string? email, bool expected)
    {
        var actual = DisposableEmailDetector.IsDisposableEmail(email);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow(null, false)]
    [DataRow("", false)]
    [DataRow("x", false)]
    [DataRow("gmail", false)]
    [DataRow("gmail.com", false)]
    [DataRow("gMail.cOm", false)]
    [DataRow("0-mail.com", true)]
    [DataRow("0-MaIL.cOm", true)]
    public void IsDisposableEmailDomainTest(string? email, bool expected)
    {
        var actual = DisposableEmailDetector.IsDisposableEmailDomain(email);
        Assert.AreEqual(expected, actual);
    }
}

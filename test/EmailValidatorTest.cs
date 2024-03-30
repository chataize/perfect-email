namespace ChatAIze.PerfectEmail.Test;

[TestClass]
public class EmailValidatorTest
{
    [DataTestMethod]
    [DataRow(null, false)]
    [DataRow("", false)]
    [DataRow("        ", false)]
    [DataRow("a", false)]
    [DataRow("@", false)]
    [DataRow(".@", false)]
    [DataRow("@.", false)]
    [DataRow(".@.", false)]
    [DataRow(".@.a", false)]
    [DataRow("-@a.aa", false)]
    [DataRow("+@a.aa", false)]
    [DataRow("a.@.a", false)]
    [DataRow("a.@.aa", false)]
    [DataRow("a.aa@.aa", false)]
    [DataRow("a.aa@a.a", false)]
    [DataRow("a.aa@aa.a", false)]
    [DataRow("a.aa@a..a", false)]
    [DataRow("a.aa@aa.aa.a", false)]
    [DataRow(" a@a.aa", false)]
    [DataRow("a@a.aa ", false)]
    [DataRow("a@a.aa.", false)]
    [DataRow("a@a.a1", false)]
    [DataRow("a@a.11", false)]
    [DataRow("a@a.aa", true)]
    [DataRow("a@aa.aa", true)]
    [DataRow("a@aa.a.aa", true)]
    [DataRow(".@a.aa", true)]
    [DataRow("a.a@a.aa", true)]
    [DataRow("a.a@a1.aa", true)]
    [DataRow("a..a@a.aa", true)]
    [DataRow(".a..a@a.aa", true)]
    [DataRow("a-a@a.aa", true)]
    [DataRow("a--a@a.aa", true)]
    [DataRow("a+a@a.aa", true)]
    [DataRow("a++a@a.aa", true)]
    public void IsValidEmailTest(string? email, bool expected)
    {
        var result = EmailValidator.IsValidEmail(email);
        Assert.AreEqual(result, expected);
    }
}

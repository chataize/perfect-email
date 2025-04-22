namespace ChatAIze.PerfectEmail.Test
{
    [TestClass]
    public sealed class EmailNormalizerTest
    {
        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow(" ", null)]
        [DataRow("a", null)]
        [DataRow("@", null)]
        [DataRow("a@", null)]
        [DataRow("@a", null)]
        [DataRow("a@a", null)]
        [DataRow("a@a.a", null)]
        [DataRow(".", null)]
        [DataRow("a.", null)]
        [DataRow(".a", null)]
        [DataRow("a.a", null)]
        [DataRow("@a.a", null)]
        [DataRow("a@a.aa", "a@a.aa")]
        [DataRow(" a@a.aa", "a@a.aa")]
        [DataRow("a@a.aa ", "a@a.aa")]
        [DataRow(" a@a.aa ", "a@a.aa")]
        [DataRow("a+a@a.aa", "a@a.aa")]
        [DataRow(" a+a@a.aa", "a@a.aa")]
        [DataRow("a+a@a.aa ", "a@a.aa")]
        [DataRow(" a+a@a.aa ", "a@a.aa")]
        [DataRow("a+aa@a.aa", "a@a.aa")]
        [DataRow("aa+a@a.aa", "aa@a.aa")]
        [DataRow("aa+aa@a.aa", "aa@a.aa")]
        public void NormalizeEmailTest(string email, object expected)
        {
            var actual = EmailNormalizer.NormalizeEmail(email);
            Assert.AreEqual(expected, actual);
        }
    }
}

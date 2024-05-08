using CGLibs._2FA.OTP;
using CGLibs._2FA.TOTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CGLibs._2FA.Encoding;
using CGLibs._2FA.Imaging;

namespace CGLibs._2FA.UnitTest
{
    [TestClass]
    public class OTPGeneratorTests
    {
        public OTPGeneratorTests()
        {

        }

        string mysecret = "$ecretKeyForTestingPurposesThatIsReallyLongAndCrazyToTryAndUse";

        [TestMethod]
        public void GenerateSha1SixDigitCode()
        {
            try
            {
                Console.WriteLine("Starting code generation");
                var otp = new OTPGenerator(mysecret);

                var result = otp.Generate(2);

                Console.WriteLine(result);

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GenerateSha1SixDigitCodeDate()
        {
            try
            {
                DateTime time = DateTime.UtcNow;

                Console.WriteLine("Starting code generation");
                var otp = new TOTPGenerator(mysecret);

                var result = otp.GenerateForDate(time);

                Console.WriteLine(result);

                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ValidateSha1SixDigitCodeDate()
        {
            try
            {
                var otp = new TOTPValidator(mysecret);

                var result = otp.GenerateForDate();

                Console.WriteLine($"Retrieved code: {result}");

                var results = otp.GetValidTotps(TimeSpan.FromSeconds(240));

                foreach (var r in results)
                {
                    Console.WriteLine($"Validation code: {r}");
                }

                bool isvalid = otp.Validate(result, timeToleranceInSeconds: 30);

                Assert.IsTrue(isvalid);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ValidateSha1EightDigitCodeDate()
        {
            try
            {
                var otp = new TOTPValidator(mysecret, digits: 8);

                var result = otp.GenerateForDate();

                Console.WriteLine($"Retrieved code: {result}");

                var results = otp.GetValidTotps(TimeSpan.FromSeconds(240));

                foreach (var r in results)
                {
                    Console.WriteLine($"Validation code: {r}");
                }

                bool isvalid = otp.Validate(result, timeToleranceInSeconds: 30);

                Assert.IsTrue(isvalid);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GenerateBase32String()
        {
            string output = mysecret.ToBase32String();

            Console.WriteLine(output);

            Assert.IsNotNull(output);

        }

        [TestMethod]
        public void TestImplicitConversionTOTP()
        {
            string typeval = "totp";
            AuthenticatorType mytype = typeval;
            Console.WriteLine(mytype.ToString());
            Assert.AreEqual<string>(typeval, mytype);
        }

        [TestMethod]
        public void TestImplicitConversionHOTP()
        {
            string typeval = "hotp";
            AuthenticatorType mytype = typeval;
            Console.WriteLine(mytype.ToString());
            Assert.AreEqual<string>(typeval, mytype);
        }

        [TestMethod]
        public void TestImplicitConversionINVALID()
        {
            string typeval = "INVALID";
            AuthenticatorType mytype = typeval;
            Assert.IsNull(mytype);
        }

    }
}

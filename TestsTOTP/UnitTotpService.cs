using TOTP.Services;
using TestsTOTP.Models;

// https://docs.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022

namespace TestsTOTP
{
    [TestClass]
    public class UnitTestsTOTP
    {
        protected readonly string filepath = "../../../config.ini";

        [TestMethod]
        public void Totp_Success()
        {
            Assert.AreEqual("II1E0HMXN37HD63SOLN9D33V2GP4EQJAL53PYVRGQAPN8EQPV4", this.returnCorrectSecret());
            TotpModel[] tests = returnGoodTestValues();

            foreach (TotpModel test in tests)
            {
                String username = test.Username;
                int unixtimestamp = test.Timestamp;
                String expected_totp = test.Totp;

                TestTotpValues(username, unixtimestamp, expected_totp);
            }           
        }

        [TestMethod]
        public void Totp_Fail_WrongTime()
        {
            TotpModel[] tests = returnGoodTestValues();

            foreach (TotpModel test in tests)
            {
                String username = test.Username;
                int unixtimestamp = test.Timestamp;
                String expected_totp = test.Totp;

                TestTotpValues(username, unixtimestamp + 30, expected_totp, false);
                TestTotpValues(username, unixtimestamp - 30, expected_totp, false);
            }
        }

        [TestMethod]
        public void Totp_Fail_WrongSecret()
        {
            TotpModel[] tests = returnGoodTestValues();

            foreach (TotpModel test in tests)
            {
                String username = test.Username;
                int unixtimestamp = test.Timestamp;
                String expected_totp = test.Totp;

                TestTotpValues(username, unixtimestamp, expected_totp, false, false);
            }
        }

        [TestMethod]
        public void Totp_Fail_WrongValues()
        {
            TotpModel[] tests = returnBadTestValues();

            foreach (TotpModel test in tests)
            {
                String username = test.Username;
                int unixtimestamp = test.Timestamp;
                String expected_totp = test.Totp;

                TestTotpValues(username, unixtimestamp, expected_totp, false);
            }
        }

        public void TestTotpValues(String username, int unixtimestamp, String expected_totp, bool expect_success = true, bool secret_type = true)
        {
            TotpService totpService = generateTotpService(secret_type);

            for (int i = 0; i < 30; i++)
            {
                int new_unixtimestamp = unixtimestamp + i;
                String totp = totpService.GenerateTotp(username, new_unixtimestamp);

                if (expect_success)
                {
                    Assert.AreEqual(expected_totp, totp);
                } else
                {
                    Assert.AreNotEqual(expected_totp, totp);
                }
                
            }
        }


        [TestMethod]
        public void ExpiryTime_Success()
        {
            TotpModel[] tests = returnGoodTestValues();

            foreach (TotpModel test in tests)
            {
                int unixtimestamp = test.Timestamp;
                int expected_expirytime = test.ExpectedExpiryTime;

                TestExpiryTimeValues(unixtimestamp, expected_expirytime);
            }
        }

        [TestMethod]
        public void ExpiryTime_Fail_WrongTime()
        {
            TotpModel[] tests = returnBadTestValues();

            foreach (TotpModel test in tests)
            {
                int unixtimestamp = test.Timestamp;
                int expected_expirytime = test.ExpectedExpiryTime;

                TestExpiryTimeValues(unixtimestamp, expected_expirytime, false);
            }
        }

        public void TestExpiryTimeValues(int unixtimestamp, int expected_expiryTime, bool expect_success = true)
        {
            TotpService totpService = generateTotpService();

            for (int i = 0; i < 30; i++)
            {
                int new_unixtimestamp = unixtimestamp + i;
                int expiryTime = totpService.GenerateExpiryTime(new_unixtimestamp);

                if (expect_success)
                {
                    Assert.AreEqual(expected_expiryTime, expiryTime);
                } else
                {
                    Assert.AreNotEqual(expected_expiryTime, expiryTime);
                }                
            }
        }

        protected internal TotpService generateTotpService(bool secret_type = true)
        {
            TotpService totpService = new TotpService(secret_type ? returnCorrectSecret() : returnWrongSecret());

            return totpService;
        }

        protected internal TotpModel[] returnGoodTestValues()
        {
            return new TotpModel[]
            {
                new TotpModel("dani", 1657974210, "338422", 1657974240),
                new TotpModel("dani1", 1657974690, "073890", 1657974720),
                new TotpModel("dani2", 1657984740, "665866", 1657984770),
            };
        }

        protected internal TotpModel[] returnBadTestValues()
        {
            return new TotpModel[]
            {
                new TotpModel("dani", 1657974210, "338421", 1657974241),
                new TotpModel("dani1", 1657974690, "073892", 1657974731),
                new TotpModel("dani2", 1657984740, "665867", 1657984771),
            };
        }

        protected internal String returnCorrectSecret() 
        {
            return GetIniKey(this.filepath, "correct_secret");
        }
        protected internal String returnWrongSecret()
        {
            return GetIniKey(this.filepath, "wrong_secret");
        }

        protected internal String GetIniKey(String filename, String key)
        {
            var lines = File.ReadAllLines(filename);
            var dict = new Dictionary<string, string>();

            foreach (var s in lines)
            {
                var split = s.Split("=");
                dict.Add(split[0], split[1]);
            }

            return dict[key];
        }
    }
}
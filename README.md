# cglibs-2fa
OTP, HOTP and TOTP library for .NET. Create and validate 6-8 digit codes.


# Usage

### Create a new instance of the Authenticator class and create a qr code image for the user to scan.
```csharp
CGLibs._2FA.Imaging.Authenticator auth = new CGLibs._2FA.Imaging.Authenticator();
auth.Width = 500;
auth.Height = 500;
auth.Period = 60;

pictureBox1.Image = auth.GetQRCodeImage("$ecretKeyForTestingPurposesThatIsReallyLongAndCrazyToTryAndUse", 
	"Issuer", "UserAccount@SomeDomainOrAppEtc");
```

### Generate a code
```csharp
TOTPGenerator totpGenerator = new CGLibs._2FA.TOTP.TOTPGenerator("SecretKey");
string code = totpGenerator.GenerateForDate();
```

### Validate a code

```csharp
string usercode = "123456";
TOTPValidator totpValidator = new CGLibs._2FA.TOTP.TOTPValidator("SecretKey");
bool isValid = totpGenerator.Validate(usercode);	
```



### Credits
Thanks to `ZXing.Net` for QR code generation! 
[https://github.com/micjahn/ZXing.Net]()





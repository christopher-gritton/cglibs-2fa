namespace CGLibs._2FA.Imaging
{
    public class AuthenticatorAlgorithm 
    {
        private AuthenticatorAlgorithm(string algorithm)
        {
            _algorithm = algorithm.ToUpper();
        }

        private string _algorithm = "SHA1";

        public static implicit operator string(AuthenticatorAlgorithm algorithm)
        {
            return algorithm._algorithm;
        }

        public static implicit operator AuthenticatorAlgorithm(string algorithm)
        {
            if (algorithm.Equals("sha1", System.StringComparison.OrdinalIgnoreCase) || algorithm.Equals("sha256", System.StringComparison.OrdinalIgnoreCase) || algorithm.Equals("sha512", System.StringComparison.OrdinalIgnoreCase))
            {
                return new AuthenticatorAlgorithm(algorithm);
            }
            else
            {
                return null;
            }
        }

        public static AuthenticatorAlgorithm SHA1 = new AuthenticatorAlgorithm("SHA1");
        public static AuthenticatorAlgorithm SHA256 = new AuthenticatorAlgorithm("SHA256");
        public static AuthenticatorAlgorithm SHA512 = new AuthenticatorAlgorithm("SHA512");

    }
}

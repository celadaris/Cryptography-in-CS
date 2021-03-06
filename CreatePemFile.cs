using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;

namespace CreatePemFiles
{
    class Program
    {
        const string publicKeyFileLocation = @"C:\Users\x\Documents\PublicKey.pem";
        const string privateKeyFileLocation = @"C:\Users\x\Documents\PrivateKey.pem";

        static void Main(string[] args)
        {
            //rSAKeyPairGenerator generates the RSA key pair based on the random number and strength of the key required
            RsaKeyPairGenerator rSAKeyPair = new RsaKeyPairGenerator();
            SecureRandom secureRandom = new SecureRandom();
            secureRandom.SetSeed(secureRandom.GenerateSeed(1000));
            rSAKeyPair.Init(new KeyGenerationParameters(secureRandom, 2048));
            AsymmetricCipherKeyPair keyPair = rSAKeyPair.GenerateKeyPair();

            //Extract private/public key from the pair
            RsaKeyParameters privateKey = keyPair.Private as RsaKeyParameters;
            RsaKeyParameters publicKey = keyPair.Public as RsaKeyParameters;

            //print public & private key in pem format
            CreatePem(publicKey);
            CreatePem(privateKey);

        }
        
        static void CreatePem(RsaKeyParameters key)
        {
            TextWriter textWriter = new StringWriter();
            PemWriter pemWriter = new PemWriter(textWriter);
            string printKey;
            string fileLocation;

            pemWriter.WriteObject(key);
            pemWriter.Writer.Flush();
            printKey = textWriter.ToString();

            if (key.IsPrivate)
            {
                fileLocation = privateKeyFileLocation;
            }
            else
            {
                fileLocation = publicKeyFileLocation;
            }

            File.WriteAllText(fileLocation, printKey);
        }
    }
}

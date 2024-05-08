using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_Graphics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                CGLibs._2FA.Imaging.Authenticator auth = new CGLibs._2FA.Imaging.Authenticator();
                auth.Width = 500;
                auth.Height = 500;
                auth.Period = 60;

                pictureBox1.Image = auth.GetQRCodeImage("$ecretKeyForTestingPurposesThatIsReallyLongAndCrazyToTryAndUse", "Issuer", "User@CustomApp");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

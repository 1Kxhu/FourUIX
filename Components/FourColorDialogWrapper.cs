using FourUIX.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FourUIX.Components
{
    public partial class FourColorDialogWrapper : Component
    {
        public FourColorDialogWrapper()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog()
        {
            InitializeComponent();
            fourColorDialog = new FourColorDialog();
            DialogResult result = fourColorDialog.ShowDialog();
            fourColorDialog = null;
            return result;
        }

        public FourColorDialog fourColorDialog = new FourColorDialog();

        private int buttonCornerRadius = 5;
        private int cornerRadius = 4;

       
        [Category("Appearance")]
        public int ButtonCornerRadius
        {
            get { return buttonCornerRadius; }
            set { buttonCornerRadius = value; fourColorDialog.buttonCornerRadius = value; }
        }

        [Category("Appearance")]
        public int CornerRadius
        {
            get { return cornerRadius; }
            set { cornerRadius = value; fourColorDialog.cornerRadius = value; }
        }

       

        public FourColorDialogWrapper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();


        }
    }
}

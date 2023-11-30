/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  

Written by mrelive@mrelive.com
*****************************************************************************/
using System.Linq;
using System.IO;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rug.Osc;
using DirectShowLib;
using System.Collections.Generic;


namespace ECHOOSC
{

    

    public class Form1 : System.Windows.Forms.Form
    {
        public NotifyIcon notifyIcon1;
        public Dictionary<string, string> DeviceIdToName { get; set; } = new Dictionary<string, string>();
        public List<Profile> Profiles { get; set; } = new List<Profile>();
        public List<CameraPosition> CameraPositions { get; set; } = new List<CameraPosition>();
        private ComboBox comboDevice;
        private Button buttonDump;
        private Button buttonStopServer;
        private Button buttonStartServer;
        private Button buttonUp;
        private Button buttonDown;
        private Button buttonLeft;
        private Button buttonRight;
        private Button buttonCenter;
        private Button buttonRightLimit;
        private Button buttonLeftLimit;
        private Button buttonUpLimit;
        private Button buttonDownLimit;
        private Button buttonZoomOutFull;
        private Button buttonZoomInFull;
        private Button buttonZoomOut;
        private Button buttonZoomIn;
        private Button buttonFastLeft;
        private Button buttonFastRight;
        private Button buttonFastDown;
        private Button buttonFastUp;
        private TextBox textPort;
        private Label label1;
        private Label label2;
        public TextBox textLastMessage;
        private Button SaveProfile;
        private Button ClearProfile;
        private ComboBox PROFILEBOX;
        public ContextMenuStrip contextMenuStrip1;
        public ToolStripMenuItem StartServerNotif;
        public ToolStripMenuItem StoptServerNotif;
        public ToolStripTextBox PortNotifBox;
        public ToolStripComboBox ComboBoxNotif;
        public PictureBox pictureBox1;
        public ComboBox POSBOX;
        private Button DELPOS;
        private Button SAVEPOS;
        private IContainer components;
public class Profile
{
     public string DeviceId { get; set; }
    public int Port { get; set; }
}public class CameraPosition
{
    public int Pan { get; set; }
    public int Tilt { get; set; }
    public int Zoom { get; set; }
}

        //A (modified) definition of OleCreatePropertyFrame found here: http://groups.google.no/group/microsoft.public.dotnet.languages.csharp/browse_thread/thread/db794e9779144a46/55dbed2bab4cd772?lnk=st&q=[DllImport(%22olepro32.dll%22)]&rnum=1&hl=no#55dbed2bab4cd772
        [DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int OleCreatePropertyFrame(
            IntPtr hwndOwner,
            int x,
            int y,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
            int cObjects,
            [MarshalAs(UnmanagedType.Interface, ArraySubType=UnmanagedType.IUnknown)]
            ref object ppUnk,
            int cPages,
            IntPtr lpPageClsID,
            int lcid,
            int dwReserved,
            IntPtr lpvReserved);

        public IBaseFilter theDevice = null;
        public string theDevicePath = "";
        CameraControl camControl = null;


        public Form1()
        {
            
              InitializeComponent();
    Console.WriteLine("Before calling LoadProfilesFromFile");
    LoadProfilesFromFile();
    Console.WriteLine("After calling LoadProfilesFromFile");
    UpdateProfileList();
    LoadProfilesFromFile();
    UpdateProfileList();
    UpdatePositionList();
 
             notifyIcon1.Text = "ECHO OSC NOT STARTED.";

           
Dictionary<string, int> deviceNameCount = new Dictionary<string, int>();

foreach (DsDevice device in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
{
    if (device.Name.Contains("OBSBOT Tiny"))
    { object source = null;
        Guid iid = typeof(IBaseFilter).GUID;
        try
        {
            device.Mon.BindToObject(null, null, ref iid, out source);
        }
        catch (Exception ex)
        {
           
            continue;
        }

       
        if (!deviceNameCount.ContainsKey(device.Name))
        {
            deviceNameCount[device.Name] = 1;
        }
        else
        {
            deviceNameCount[device.Name]++;
        }

       
        string deviceName = device.Name;
        if (deviceNameCount[device.Name] > 1)
        {
            deviceName += " #" + deviceNameCount[device.Name];
        }

        comboDevice.Items.Add(deviceName); // Add the device name to the ComboBox
        DeviceIdToName[device.DevicePath] = deviceName; // Map the device ID to the device name
        theDevice = (IBaseFilter)source;
        theDevicePath = device.DevicePath;
    }
}

            //Select first combobox item
            if (comboDevice.Items.Count > 0)
            {
                comboDevice.SelectedIndex = 0;
            }

            //StartServer();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.comboDevice = new System.Windows.Forms.ComboBox();
            this.buttonDump = new System.Windows.Forms.Button();
            this.buttonStopServer = new System.Windows.Forms.Button();
            this.buttonStartServer = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.buttonRightLimit = new System.Windows.Forms.Button();
            this.buttonLeftLimit = new System.Windows.Forms.Button();
            this.buttonUpLimit = new System.Windows.Forms.Button();
            this.buttonDownLimit = new System.Windows.Forms.Button();
            this.buttonZoomOutFull = new System.Windows.Forms.Button();
            this.buttonZoomInFull = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonFastLeft = new System.Windows.Forms.Button();
            this.buttonFastRight = new System.Windows.Forms.Button();
            this.buttonFastDown = new System.Windows.Forms.Button();
            this.buttonFastUp = new System.Windows.Forms.Button();
            this.textPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textLastMessage = new System.Windows.Forms.TextBox();
            this.SaveProfile = new System.Windows.Forms.Button();
            this.ClearProfile = new System.Windows.Forms.Button();
            this.PROFILEBOX = new System.Windows.Forms.ComboBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.StartServerNotif = new System.Windows.Forms.ToolStripMenuItem();
            this.StoptServerNotif = new System.Windows.Forms.ToolStripMenuItem();
            this.PortNotifBox = new System.Windows.Forms.ToolStripTextBox();
            this.ComboBoxNotif = new System.Windows.Forms.ToolStripComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.POSBOX = new System.Windows.Forms.ComboBox();
            this.DELPOS = new System.Windows.Forms.Button();
            this.SAVEPOS = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboDevice
            // 
            this.comboDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(35)))), ((int)(((byte)(36)))));
            this.comboDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDevice.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboDevice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.comboDevice.Location = new System.Drawing.Point(8, 8);
            this.comboDevice.Name = "comboDevice";
            this.comboDevice.Size = new System.Drawing.Size(226, 21);
            this.comboDevice.TabIndex = 0;
            this.comboDevice.SelectedIndexChanged += new System.EventHandler(this.comboDevice_SelectedIndexChanged);
            // 
            // buttonDump
            // 
            this.buttonDump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonDump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDump.ForeColor = System.Drawing.Color.Silver;
            this.buttonDump.Location = new System.Drawing.Point(124, 157);
            this.buttonDump.Name = "buttonDump";
            this.buttonDump.Size = new System.Drawing.Size(110, 24);
            this.buttonDump.TabIndex = 2;
            this.buttonDump.Text = "Dump Settings";
            this.buttonDump.UseVisualStyleBackColor = false;
            this.buttonDump.Click += new System.EventHandler(this.buttonDump_Click);
            // 
            // buttonStopServer
            // 
            this.buttonStopServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonStopServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStopServer.ForeColor = System.Drawing.Color.Silver;
            this.buttonStopServer.Location = new System.Drawing.Point(8, 64);
            this.buttonStopServer.Name = "buttonStopServer";
            this.buttonStopServer.Size = new System.Drawing.Size(110, 24);
            this.buttonStopServer.TabIndex = 4;
            this.buttonStopServer.Text = "Stop OSC Server";
            this.buttonStopServer.UseVisualStyleBackColor = false;
            this.buttonStopServer.Click += new System.EventHandler(this.buttonStopServer_Click);
            // 
            // buttonStartServer
            // 
            this.buttonStartServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonStartServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStartServer.ForeColor = System.Drawing.Color.Silver;
            this.buttonStartServer.Location = new System.Drawing.Point(8, 35);
            this.buttonStartServer.Name = "buttonStartServer";
            this.buttonStartServer.Size = new System.Drawing.Size(110, 24);
            this.buttonStartServer.TabIndex = 5;
            this.buttonStartServer.Text = "Start OSC Server";
            this.buttonStartServer.UseVisualStyleBackColor = false;
            this.buttonStartServer.Click += new System.EventHandler(this.buttonStartServer_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUp.ForeColor = System.Drawing.Color.Silver;
            this.buttonUp.Location = new System.Drawing.Point(124, 302);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(110, 24);
            this.buttonUp.TabIndex = 7;
            this.buttonUp.Text = "Up";
            this.buttonUp.UseVisualStyleBackColor = false;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDown.ForeColor = System.Drawing.Color.Silver;
            this.buttonDown.Location = new System.Drawing.Point(124, 389);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(110, 24);
            this.buttonDown.TabIndex = 8;
            this.buttonDown.Text = "Down";
            this.buttonDown.UseVisualStyleBackColor = false;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonLeft
            // 
            this.buttonLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeft.ForeColor = System.Drawing.Color.Silver;
            this.buttonLeft.Location = new System.Drawing.Point(124, 331);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(110, 24);
            this.buttonLeft.TabIndex = 9;
            this.buttonLeft.Text = "Left";
            this.buttonLeft.UseVisualStyleBackColor = false;
            this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
            // 
            // buttonRight
            // 
            this.buttonRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRight.ForeColor = System.Drawing.Color.Silver;
            this.buttonRight.Location = new System.Drawing.Point(124, 360);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(110, 24);
            this.buttonRight.TabIndex = 10;
            this.buttonRight.Text = "Right";
            this.buttonRight.UseVisualStyleBackColor = false;
            this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
            // 
            // buttonCenter
            // 
            this.buttonCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonCenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCenter.ForeColor = System.Drawing.Color.Silver;
            this.buttonCenter.Location = new System.Drawing.Point(8, 157);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(110, 24);
            this.buttonCenter.TabIndex = 13;
            this.buttonCenter.Text = "Center";
            this.buttonCenter.UseVisualStyleBackColor = false;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonRightLimit
            // 
            this.buttonRightLimit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonRightLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRightLimit.ForeColor = System.Drawing.Color.Silver;
            this.buttonRightLimit.Location = new System.Drawing.Point(8, 360);
            this.buttonRightLimit.Name = "buttonRightLimit";
            this.buttonRightLimit.Size = new System.Drawing.Size(110, 24);
            this.buttonRightLimit.TabIndex = 14;
            this.buttonRightLimit.Text = "Right Limit";
            this.buttonRightLimit.UseVisualStyleBackColor = false;
            this.buttonRightLimit.Click += new System.EventHandler(this.buttonRightLimit_Click);
            // 
            // buttonLeftLimit
            // 
            this.buttonLeftLimit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonLeftLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeftLimit.ForeColor = System.Drawing.Color.Silver;
            this.buttonLeftLimit.Location = new System.Drawing.Point(8, 331);
            this.buttonLeftLimit.Name = "buttonLeftLimit";
            this.buttonLeftLimit.Size = new System.Drawing.Size(110, 24);
            this.buttonLeftLimit.TabIndex = 15;
            this.buttonLeftLimit.Text = "Left Limit";
            this.buttonLeftLimit.UseVisualStyleBackColor = false;
            this.buttonLeftLimit.Click += new System.EventHandler(this.buttonLeftLimit_Click);
            // 
            // buttonUpLimit
            // 
            this.buttonUpLimit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonUpLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpLimit.ForeColor = System.Drawing.Color.Silver;
            this.buttonUpLimit.Location = new System.Drawing.Point(8, 302);
            this.buttonUpLimit.Name = "buttonUpLimit";
            this.buttonUpLimit.Size = new System.Drawing.Size(110, 24);
            this.buttonUpLimit.TabIndex = 16;
            this.buttonUpLimit.Text = "Up Limit";
            this.buttonUpLimit.UseVisualStyleBackColor = false;
            this.buttonUpLimit.Click += new System.EventHandler(this.buttonUpLimit_Click);
            // 
            // buttonDownLimit
            // 
            this.buttonDownLimit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonDownLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDownLimit.ForeColor = System.Drawing.Color.Silver;
            this.buttonDownLimit.Location = new System.Drawing.Point(8, 389);
            this.buttonDownLimit.Name = "buttonDownLimit";
            this.buttonDownLimit.Size = new System.Drawing.Size(110, 24);
            this.buttonDownLimit.TabIndex = 17;
            this.buttonDownLimit.Text = "Down Limit";
            this.buttonDownLimit.UseVisualStyleBackColor = false;
            this.buttonDownLimit.Click += new System.EventHandler(this.buttonDownLimit_Click);
            // 
            // buttonZoomOutFull
            // 
            this.buttonZoomOutFull.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonZoomOutFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomOutFull.ForeColor = System.Drawing.Color.Silver;
            this.buttonZoomOutFull.Location = new System.Drawing.Point(8, 505);
            this.buttonZoomOutFull.Name = "buttonZoomOutFull";
            this.buttonZoomOutFull.Size = new System.Drawing.Size(110, 24);
            this.buttonZoomOutFull.TabIndex = 23;
            this.buttonZoomOutFull.Text = "Zoom Out Full";
            this.buttonZoomOutFull.UseVisualStyleBackColor = false;
            this.buttonZoomOutFull.Click += new System.EventHandler(this.buttonZoomOutFull_Click);
            // 
            // buttonZoomInFull
            // 
            this.buttonZoomInFull.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonZoomInFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomInFull.ForeColor = System.Drawing.Color.Silver;
            this.buttonZoomInFull.Location = new System.Drawing.Point(8, 418);
            this.buttonZoomInFull.Name = "buttonZoomInFull";
            this.buttonZoomInFull.Size = new System.Drawing.Size(110, 24);
            this.buttonZoomInFull.TabIndex = 22;
            this.buttonZoomInFull.Text = "Zoom In Full";
            this.buttonZoomInFull.UseVisualStyleBackColor = false;
            this.buttonZoomInFull.Click += new System.EventHandler(this.buttonZoomInFull_Click);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomOut.ForeColor = System.Drawing.Color.Silver;
            this.buttonZoomOut.Location = new System.Drawing.Point(8, 476);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(110, 24);
            this.buttonZoomOut.TabIndex = 21;
            this.buttonZoomOut.Text = "Zoom Out";
            this.buttonZoomOut.UseVisualStyleBackColor = false;
            this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomIn.ForeColor = System.Drawing.Color.Silver;
            this.buttonZoomIn.Location = new System.Drawing.Point(8, 447);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(110, 24);
            this.buttonZoomIn.TabIndex = 20;
            this.buttonZoomIn.Text = "Zoom In";
            this.buttonZoomIn.UseVisualStyleBackColor = false;
            this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
            // 
            // buttonFastLeft
            // 
            this.buttonFastLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonFastLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFastLeft.ForeColor = System.Drawing.Color.Silver;
            this.buttonFastLeft.Location = new System.Drawing.Point(124, 505);
            this.buttonFastLeft.Name = "buttonFastLeft";
            this.buttonFastLeft.Size = new System.Drawing.Size(110, 24);
            this.buttonFastLeft.TabIndex = 24;
            this.buttonFastLeft.Text = "Fast 10 (-160) Left";
            this.buttonFastLeft.UseVisualStyleBackColor = false;
            this.buttonFastLeft.Click += new System.EventHandler(this.buttonFastLeft_Click);
            // 
            // buttonFastRight
            // 
            this.buttonFastRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonFastRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFastRight.ForeColor = System.Drawing.Color.Silver;
            this.buttonFastRight.Location = new System.Drawing.Point(124, 476);
            this.buttonFastRight.Name = "buttonFastRight";
            this.buttonFastRight.Size = new System.Drawing.Size(110, 24);
            this.buttonFastRight.TabIndex = 25;
            this.buttonFastRight.Text = "Fast 10 (160) Right";
            this.buttonFastRight.UseVisualStyleBackColor = false;
            this.buttonFastRight.Click += new System.EventHandler(this.buttonFastRight_Click);
            // 
            // buttonFastDown
            // 
            this.buttonFastDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonFastDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFastDown.ForeColor = System.Drawing.Color.Silver;
            this.buttonFastDown.Location = new System.Drawing.Point(124, 418);
            this.buttonFastDown.Name = "buttonFastDown";
            this.buttonFastDown.Size = new System.Drawing.Size(110, 24);
            this.buttonFastDown.TabIndex = 27;
            this.buttonFastDown.Text = "Fast 11 (120) Up";
            this.buttonFastDown.UseVisualStyleBackColor = false;
            this.buttonFastDown.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonFastUp
            // 
            this.buttonFastUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.buttonFastUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFastUp.ForeColor = System.Drawing.Color.Silver;
            this.buttonFastUp.Location = new System.Drawing.Point(124, 447);
            this.buttonFastUp.Name = "buttonFastUp";
            this.buttonFastUp.Size = new System.Drawing.Size(110, 24);
            this.buttonFastUp.TabIndex = 26;
            this.buttonFastUp.Text = "Fast 11 (-120) Down";
            this.buttonFastUp.UseVisualStyleBackColor = false;
            this.buttonFastUp.Click += new System.EventHandler(this.button2_Click);
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(155, 39);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(62, 20);
            this.textPort.TabIndex = 28;
            this.textPort.Text = "33333";
            this.textPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textPort.TextChanged += new System.EventHandler(this.textPort_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(125, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(78, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Last Message:";
            // 
            // textLastMessage
            // 
            this.textLastMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(35)))), ((int)(((byte)(36)))));
            this.textLastMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textLastMessage.ForeColor = System.Drawing.Color.Lime;
            this.textLastMessage.Location = new System.Drawing.Point(14, 138);
            this.textLastMessage.Name = "textLastMessage";
            this.textLastMessage.Size = new System.Drawing.Size(205, 13);
            this.textLastMessage.TabIndex = 31;
            // 
            // SaveProfile
            // 
            this.SaveProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.SaveProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveProfile.ForeColor = System.Drawing.Color.Silver;
            this.SaveProfile.Location = new System.Drawing.Point(8, 94);
            this.SaveProfile.Name = "SaveProfile";
            this.SaveProfile.Size = new System.Drawing.Size(110, 24);
            this.SaveProfile.TabIndex = 33;
            this.SaveProfile.Text = "Save Profile";
            this.SaveProfile.UseVisualStyleBackColor = false;
            this.SaveProfile.Click += new System.EventHandler(this.buttonSaveProfile_Click);
            // 
            // ClearProfile
            // 
            this.ClearProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.ClearProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearProfile.ForeColor = System.Drawing.Color.Silver;
            this.ClearProfile.Location = new System.Drawing.Point(124, 94);
            this.ClearProfile.Name = "ClearProfile";
            this.ClearProfile.Size = new System.Drawing.Size(110, 24);
            this.ClearProfile.TabIndex = 34;
            this.ClearProfile.Text = "Clear Profile";
            this.ClearProfile.UseVisualStyleBackColor = false;
            this.ClearProfile.Click += new System.EventHandler(this.buttonClearProfile_Click);
            // 
            // PROFILEBOX
            // 
            this.PROFILEBOX.FormattingEnabled = true;
            this.PROFILEBOX.Location = new System.Drawing.Point(124, 67);
            this.PROFILEBOX.Name = "PROFILEBOX";
            this.PROFILEBOX.Size = new System.Drawing.Size(110, 21);
            this.PROFILEBOX.TabIndex = 35;
            this.PROFILEBOX.SelectedIndexChanged += new System.EventHandler(this.PROFILEBOX_SelectedIndexChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AccessibleName = "ContextMenuStrip1";
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartServerNotif,
            this.StoptServerNotif,
            this.PortNotifBox,
            this.ComboBoxNotif});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 100);
            // 
            // StartServerNotif
            // 
            this.StartServerNotif.Name = "StartServerNotif";
            this.StartServerNotif.Size = new System.Drawing.Size(181, 22);
            this.StartServerNotif.Text = "Start Server";
            this.StartServerNotif.Click += new System.EventHandler(this.StartServerNotif_Click);
            // 
            // StoptServerNotif
            // 
            this.StoptServerNotif.Name = "StoptServerNotif";
            this.StoptServerNotif.Size = new System.Drawing.Size(181, 22);
            this.StoptServerNotif.Text = "Stop Server";
            this.StoptServerNotif.Click += new System.EventHandler(this.StoptServerNotif_Click);
            // 
            // PortNotifBox
            // 
            this.PortNotifBox.AccessibleName = "PortNotif";
            this.PortNotifBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.PortNotifBox.Name = "PortNotifBox";
            this.PortNotifBox.ReadOnly = true;
            this.PortNotifBox.Size = new System.Drawing.Size(100, 23);
            this.PortNotifBox.ToolTipText = "Running On Port";
            // 
            // ComboBoxNotif
            // 
            this.ComboBoxNotif.AccessibleName = "ComboBoxNotif";
            this.ComboBoxNotif.Name = "ComboBoxNotif";
            this.ComboBoxNotif.Size = new System.Drawing.Size(121, 23);
            this.ComboBoxNotif.SelectedIndexChanged += new System.EventHandler(this.ComboBoxNotif_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleName = "POSITIONSAVE";
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(8, 214);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(104, 82);
            this.pictureBox1.TabIndex = 36;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // POSBOX
            // 
            this.POSBOX.FormattingEnabled = true;
            this.POSBOX.Location = new System.Drawing.Point(8, 187);
            this.POSBOX.Name = "POSBOX";
            this.POSBOX.Size = new System.Drawing.Size(227, 21);
            this.POSBOX.TabIndex = 37;
            this.POSBOX.SelectedIndexChanged += new System.EventHandler(this.POSBOX_SelectedIndexChanged);
            // 
            // DELPOS
            // 
            this.DELPOS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.DELPOS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DELPOS.ForeColor = System.Drawing.Color.Silver;
            this.DELPOS.Location = new System.Drawing.Point(125, 244);
            this.DELPOS.Name = "DELPOS";
            this.DELPOS.Size = new System.Drawing.Size(110, 24);
            this.DELPOS.TabIndex = 39;
            this.DELPOS.Text = "DELETE";
            this.DELPOS.UseVisualStyleBackColor = false;
            this.DELPOS.Click += new System.EventHandler(this.DELPOS_Click);
            // 
            // SAVEPOS
            // 
            this.SAVEPOS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(50)))), ((int)(((byte)(51)))));
            this.SAVEPOS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SAVEPOS.ForeColor = System.Drawing.Color.Silver;
            this.SAVEPOS.Location = new System.Drawing.Point(124, 214);
            this.SAVEPOS.Name = "SAVEPOS";
            this.SAVEPOS.Size = new System.Drawing.Size(110, 24);
            this.SAVEPOS.TabIndex = 38;
            this.SAVEPOS.Text = "SAVE POSITION";
            this.SAVEPOS.UseVisualStyleBackColor = false;
            this.SAVEPOS.Click += new System.EventHandler(this.SAVEPOS_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(35)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(247, 536);
            this.Controls.Add(this.DELPOS);
            this.Controls.Add(this.SAVEPOS);
            this.Controls.Add(this.POSBOX);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.PROFILEBOX);
            this.Controls.Add(this.ClearProfile);
            this.Controls.Add(this.SaveProfile);
            this.Controls.Add(this.textLastMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textPort);
            this.Controls.Add(this.buttonFastDown);
            this.Controls.Add(this.buttonFastUp);
            this.Controls.Add(this.buttonFastRight);
            this.Controls.Add(this.buttonFastLeft);
            this.Controls.Add(this.buttonZoomOutFull);
            this.Controls.Add(this.buttonZoomInFull);
            this.Controls.Add(this.buttonZoomOut);
            this.Controls.Add(this.buttonZoomIn);
            this.Controls.Add(this.buttonDownLimit);
            this.Controls.Add(this.buttonUpLimit);
            this.Controls.Add(this.buttonLeftLimit);
            this.Controls.Add(this.buttonRightLimit);
            this.Controls.Add(this.buttonCenter);
            this.Controls.Add(this.buttonRight);
            this.Controls.Add(this.buttonLeft);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonStartServer);
            this.Controls.Add(this.buttonStopServer);
            this.Controls.Add(this.buttonDump);
            this.Controls.Add(this.comboDevice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "ECHO OSC";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Point? lastMousePosition = null;private Point joystickPosition; // This field is used to store the joystick position

private void pictureBox1_Paint(object sender, PaintEventArgs e)
{
    // Draw a circle that represents the joystick handle
    e.Graphics.FillEllipse(Brushes.Red, joystickPosition.X - 10, joystickPosition.Y - 10, 20, 20);
}
private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
{
      // Set the joystick position to the center of the PictureBox
    joystickPosition = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);

    // Redraw the PictureBox
    pictureBox1.Invalidate();
}

private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
{
    if (e.Button == MouseButtons.Left)
    {
        // Update the joystick position
        joystickPosition = e.Location;

        // Redraw the PictureBox
        pictureBox1.Invalidate();

        // Normalize the x and y values to the range of the camera control
        int x = ((pictureBox1.Width - e.X) * 180 / pictureBox1.Width) - 90; // Assuming the range is -90 to 90
        int y = ((pictureBox1.Height - e.Y) * 180 / pictureBox1.Height) - 90; // Assuming the range is -90 to 90

        // Set the camera control
        SetCameraControl(x, y);
    }

        }private void ComboBoxNotif_SelectedIndexChanged(object sender, EventArgs e)
{
    PROFILEBOX.SelectedIndex = ComboBoxNotif.SelectedIndex;
}private void StartServerNotif_Click(object sender, EventArgs e)
{
    StartServer();
}

private void StoptServerNotif_Click(object sender, EventArgs e)
{
    StopServer();
}

private void textPort_TextChanged(object sender, EventArgs e)
{
    PortNotifBox.Text = textPort.Text;
}

private void PROFILEBOX_SelectedIndexChanged(object sender, EventArgs e)
{
    if (PROFILEBOX.SelectedIndex >= 0 && PROFILEBOX.SelectedIndex < Profiles.Count)
    {
        ComboBoxNotif.SelectedIndex = PROFILEBOX.SelectedIndex;
        LoadSelectedProfile();
    }
}private void POSBOX_SelectedIndexChanged(object sender, EventArgs e)
{
    if (POSBOX.SelectedIndex >= 0 && POSBOX.SelectedIndex < CameraPositions.Count)
    {
        CameraPosition selectedPosition = CameraPositions[POSBOX.SelectedIndex];

        var cameraControl = theDevice as IAMCameraControl;
        if (cameraControl != null)
        {
            cameraControl.Set(CameraControlProperty.Pan, selectedPosition.Pan, CameraControlFlags.Manual);
            cameraControl.Set(CameraControlProperty.Tilt, selectedPosition.Tilt, CameraControlFlags.Manual);
            cameraControl.Set(CameraControlProperty.Zoom, selectedPosition.Zoom, CameraControlFlags.Manual);
        }
    }
}

        private void SetCameraControl(int x, int y)
{
    var cameraControl = theDevice as IAMCameraControl;
    if (cameraControl != null)
    {
        cameraControl.Set((CameraControlProperty)10, x, CameraControlFlags.Manual);
        cameraControl.Set((CameraControlProperty)11, y, CameraControlFlags.Manual);
    }
}
   
protected override void OnResize(EventArgs e)
{
    if (this.WindowState == FormWindowState.Minimized)
    {
        this.Hide();
    }

    base.OnResize(e);
}private void LoadSelectedProfile()
{
    if (PROFILEBOX.SelectedIndex >= 0 && PROFILEBOX.SelectedIndex < Profiles.Count)
    {
        Profile selectedProfile = Profiles[PROFILEBOX.SelectedIndex];
        comboDevice.SelectedItem = selectedProfile.DeviceId;
        textPort.Text = selectedProfile.Port.ToString();
    }
}
private void LoadProfilesFromFile()
{
    if (File.Exists("profiles.json"))
    {
        var profilesJson = File.ReadAllText("profiles.json");
        Profiles = JsonConvert.DeserializeObject<List<Profile>>(profilesJson);
    }
    else
    {
        Profiles = new List<Profile>();
    }

    if (File.Exists("positions.json"))
    {
        var positionsJson = File.ReadAllText("positions.json");
        CameraPositions = JsonConvert.DeserializeObject<List<CameraPosition>>(positionsJson);
    }
    else
    {
        CameraPositions = new List<CameraPosition>();
    }
}
private void UpdatePositionList()
{
    POSBOX.Items.Clear();
    for (int i = 0; i < CameraPositions.Count; i++)
    {
        var position = CameraPositions[i];
        string positionName = $"POS #{i + 1} (Pan: {position.Pan}, Tilt: {position.Tilt}, Zoom: {position.Zoom})";
        POSBOX.Items.Add(positionName);
    }
}
private void buttonClearProfile_Click(object sender, EventArgs e)
{
    Console.WriteLine($"Clear Profile button clicked. Selected index: {PROFILEBOX.SelectedIndex}");
    if (PROFILEBOX.SelectedIndex != -1)
    {
        Profiles.RemoveAt(PROFILEBOX.SelectedIndex);
        SaveProfilesToFile();
        UpdateProfileList();
        Console.WriteLine($"Profile removed. New profile count: {Profiles.Count}");
    }
}

private void UpdateProfileList()
{
    if (Profiles == null)
    {
        Console.WriteLine("Profiles is null");
        return;
    }

    PROFILEBOX.Items.Clear();
    ComboBoxNotif.Items.Clear();
    for (int i = 0; i < Profiles.Count; i++)
    {
        string profileName = $"PROFILE {i + 1}";
        PROFILEBOX.Items.Add(profileName);
        ComboBoxNotif.Items.Add(profileName);
    }
}

private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
{
    this.Show();
    this.WindowState = FormWindowState.Normal;
}
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            Application.Run(new Form1());


        }
        

public void StartServer()
{
    buttonStartServer.Enabled = false;
    buttonStopServer.Enabled = true;
    textPort.Enabled = false;
    camControl = new CameraControl(int.Parse(this.textPort.Text), this);

    // Get the friendly name for the selected device
    string deviceName = comboDevice.SelectedItem.ToString();

    // Set the Text property
    notifyIcon1.Text = $"ECHO OSC-{textPort.Text} Cam-{deviceName}.";
}

public void StopServer()
{
    buttonStartServer.Enabled = true;
    buttonStopServer.Enabled = false;
    textPort.Enabled = true;
    if (camControl != null)
        camControl.stop();

    // Set the Text property
    notifyIcon1.Text = "ECHO OSC NOT STARTED.";
}

        private IBaseFilter CreateFilter(Guid category, string dpath)
        {
            object source = null;
            Guid iid = typeof(IBaseFilter).GUID;
            foreach (DsDevice device in DsDevice.GetDevicesOfCat(category))
            {
                if (device.DevicePath.CompareTo(dpath) == 0)
                {
                    device.Mon.BindToObject(null, null, ref iid, out source);
                    break;
                }
            }

            return (IBaseFilter)source;
        }

        private void dumpAll()
        {
            object source = null;
            IBaseFilter idevice = null;
            //var cameraControl = null;
            Guid iid = typeof(IBaseFilter).GUID;
            foreach (DsDevice device in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
            {
                try
                {
                    device.Mon.BindToObject(null, null, ref iid, out source);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"device failed: {device.Name}:");
                    //Console.WriteLine(ex.ToString());
                    continue;
                }

                idevice = (IBaseFilter)source;
                var cameraControl = idevice as IAMCameraControl;
                if (cameraControl == null) continue;

                Console.WriteLine($"device: {device.Name}:");
                Console.WriteLine($"device path: {device.DevicePath}:");
                for (int i = 0; i <= 100; i++)
                {
                    int result = cameraControl.GetRange((CameraControlProperty)i,
                        out int min, out int max, out int steppingDelta,
                        out int defaultValue, out var flags);

                    if (result == 0)
                    {
                        Console.WriteLine($"Property: {i}, min: {min}, max: {max}, steppingDelta: {steppingDelta}");
                        Console.WriteLine($"defaultValue: {defaultValue}, flags: {flags}\n");

                        cameraControl.Get((CameraControlProperty)i, out int value, out var flags2);
                        Console.WriteLine($"currentValue: {value}, flags: {flags2}\n");
                    }
                }
                Console.WriteLine("-----------------------");


            }
        }

        private void dumpSettings()
{
    object source = null;
    IBaseFilter idevice = null;
    Guid iid = typeof(IBaseFilter).GUID;
    string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OBSBOT_Tiny_Settings.txt");

    // Clear the file if it already exists
    if (File.Exists(filePath))
    {
        File.WriteAllText(filePath, String.Empty);
    }

    foreach (DsDevice device in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
    {
        if (device.Name.Contains("OBSBOT Tiny"))
        {
            try
            {
                device.Mon.BindToObject(null, null, ref iid, out source);
            }
            catch (Exception ex)
            {
                // If there's an error, skip to the next device
                continue;
            }

            idevice = (IBaseFilter)source;
            var cameraControl = idevice as IAMCameraControl;
            if (cameraControl == null) continue;

            // Append the device name to the file
            File.AppendAllText(filePath, $"Device: {device.Name}:\n");

            foreach (CameraControlProperty i in Enum.GetValues(typeof(CameraControlProperty)))
            {
                cameraControl.GetRange(i,
                    out int min, out int max, out int steppingDelta,
                    out int defaultValue, out var flags);

                // Append the property details to the file
                File.AppendAllText(filePath, $"Property: {i}, min: {min}, max: {max}, steppingDelta: {steppingDelta}\n");
                File.AppendAllText(filePath, $"defaultValue: {defaultValue}, flags: {flags}\n");

                cameraControl.Get(i, out int value, out var flags2);

                // Append the current value and flags to the file
                File.AppendAllText(filePath, $"currentValue: {value}, flags: {flags2}\n");
            }

            // Append a separator line to the file
            File.AppendAllText(filePath, "-----------------------\n");
        }
    }
}

        private void buttonDump_Click(object sender, System.EventArgs e)
{
    dumpSettings();
}

        private void buttonStartServer_Click(object sender, System.EventArgs e)
        {
            StartServer();
        }

        private void buttonStopServer_Click(object sender, System.EventArgs e)
        {
            StopServer();
        }

        private void comboDevice_SelectedIndexChanged(object sender, System.EventArgs e)
{
    //Release COM objects
    if (theDevice != null)
    {
        Marshal.ReleaseComObject(theDevice);
        theDevice = null;
    }
    //Get the device ID from the dictionary using the selected friendly name
    string deviceId = DeviceIdToName.FirstOrDefault(x => x.Value == comboDevice.SelectedItem.ToString()).Key;
    //Create the filter for the selected video input device
    theDevice = CreateFilter(FilterCategory.VideoInputDevice, deviceId);
    theDevicePath = deviceId;
}
        private void buttonUp_Click(object sender, EventArgs e)
        {
            Up();
        }

        private void Up()
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get(CameraControlProperty.Tilt, out int value, out var flags);
            cameraControl.Set(CameraControlProperty.Tilt, value - 10, CameraControlFlags.Manual);
            Console.WriteLine($"Property: {CameraControlProperty.Tilt}, value: {value}");

        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get(CameraControlProperty.Tilt, out int value, out var flags);
            cameraControl.Set(CameraControlProperty.Tilt, value + 10, CameraControlFlags.Manual);
            Console.WriteLine($"Property: {CameraControlProperty.Tilt}, value: {value}");
        }

        public void farLeft()
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Set((CameraControlProperty)10, 160, CameraControlFlags.Manual);
            //Console.WriteLine($"Property: {CameraControlProperty.Tilt}, value: {value}");
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get(CameraControlProperty.Pan, out int value, out var flags);
            cameraControl.Set(CameraControlProperty.Pan, value - 10, CameraControlFlags.Manual);
            Console.WriteLine($"Property: {CameraControlProperty.Pan}, value: {value}");
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get(CameraControlProperty.Pan, out int value, out var flags);
            cameraControl.Set(CameraControlProperty.Pan, value + 10, CameraControlFlags.Manual);
            Console.WriteLine($"Property: {CameraControlProperty.Pan}, value: {value}");
        }



        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get(CameraControlProperty.Zoom, out int value, out var flags);
            cameraControl.Set(CameraControlProperty.Zoom, value + 10, CameraControlFlags.Manual);
            Console.WriteLine($"Property: {CameraControlProperty.Zoom}, value: {value}");

        }

        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get(CameraControlProperty.Zoom, out int value, out var flags);
            cameraControl.Set(CameraControlProperty.Zoom, value - 10, CameraControlFlags.Manual);
            Console.WriteLine($"Property: {CameraControlProperty.Zoom}, value: {value}");

        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.GetRange(CameraControlProperty.Pan,
                out int min, out int max, out int steppingDelta,
                out int defaultValue, out var flags);
            cameraControl.Set(CameraControlProperty.Pan, 0, CameraControlFlags.Manual);

            cameraControl.GetRange(CameraControlProperty.Tilt,
                out min, out max, out steppingDelta,
                out defaultValue, out flags);
            cameraControl.Set(CameraControlProperty.Tilt, defaultValue, CameraControlFlags.Manual);

            cameraControl.GetRange(CameraControlProperty.Zoom,
                out min, out max, out steppingDelta,
                out defaultValue, out flags);
            cameraControl.Set(CameraControlProperty.Zoom, defaultValue, CameraControlFlags.Manual);
        }

        private void buttonUpLimit_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.GetRange(CameraControlProperty.Tilt,
                out int min, out int max, out int steppingDelta,
                out int defaultValue, out var flags);
            cameraControl.Set(CameraControlProperty.Tilt, 60, CameraControlFlags.Manual);

        }

        private void buttonDownLimit_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.GetRange(CameraControlProperty.Tilt,
                out int min, out int max, out int steppingDelta,
                out int defaultValue, out var flags);
            cameraControl.Set(CameraControlProperty.Tilt, -60, CameraControlFlags.Manual);
        }

        private void buttonLeftLimit_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            //cameraControl.GetRange(CameraControlProperty.Pan,
            //    out int min, out int max, out int steppingDelta,
            //    out int defaultValue, out var flags);
            cameraControl.Set((CameraControlProperty)10, 160, CameraControlFlags.Manual);
            cameraControl.Set(CameraControlProperty.Pan, -100, CameraControlFlags.Manual);

            //Console.WriteLine($"Property: {CameraControlProperty.Pan}, min: {min}, MAX: {max}, steppingDelta: {steppingDelta}");
        }
        private void buttonRightLimit_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            //cameraControl.GetRange(CameraControlProperty.Pan,
            //    out int min, out int max, out int steppingDelta,
            //    out int defaultValue, out var flags);
            cameraControl.Set((CameraControlProperty)10, 160, CameraControlFlags.Manual);
            cameraControl.Set(CameraControlProperty.Pan, 100, CameraControlFlags.Manual);
            //Console.WriteLine($"Property: {CameraControlProperty.Pan}, MIN: {min}, max: {max}, steppingDelta: {steppingDelta}");


        }

        private void buttonZoomInFull_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.GetRange(CameraControlProperty.Zoom,
                out int min, out int max, out int steppingDelta,
                out int defaultValue, out var flags);
            cameraControl.Set(CameraControlProperty.Zoom, max, CameraControlFlags.Manual);

        }

        private void buttonZoomOutFull_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.GetRange(CameraControlProperty.Zoom,
                out int min, out int max, out int steppingDelta,
                out int defaultValue, out var flags);
            cameraControl.Set(CameraControlProperty.Zoom, min, CameraControlFlags.Manual);

        }

        private void buttonFastZoomInFull_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Set((CameraControlProperty)13, 100, CameraControlFlags.Manual);

        }

        private void buttonFastZoomOutFull_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Set((CameraControlProperty)13, -100, CameraControlFlags.Manual);

        }
        private void button2_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get((CameraControlProperty)11, out int value, out var flags);
            cameraControl.Set((CameraControlProperty)10, 0, CameraControlFlags.Manual);
            cameraControl.Set((CameraControlProperty)11, -120, CameraControlFlags.Manual);
            //Console.WriteLine($"Property 11: value: {value}");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get((CameraControlProperty)11, out int value, out var flags);
            cameraControl.Set((CameraControlProperty)10, 0, CameraControlFlags.Manual);
            cameraControl.Set((CameraControlProperty)11, 120, CameraControlFlags.Manual);
            //Console.WriteLine($"Property 11: value: {value}");

        }

        private void buttonFastLeft_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get((CameraControlProperty)10, out int value, out var flags);
            cameraControl.Set((CameraControlProperty)11, 0, CameraControlFlags.Manual);
            cameraControl.Set((CameraControlProperty)10, 160, CameraControlFlags.Manual);
            Console.WriteLine($"Property 10: value: {value}");
        }

        private void buttonFastRight_Click(object sender, EventArgs e)
        {
            var cameraControl = theDevice as IAMCameraControl;
            if (cameraControl == null) return;

            cameraControl.Get((CameraControlProperty)10, out int value, out var flags);
            cameraControl.Set((CameraControlProperty)11, 0, CameraControlFlags.Manual);
            cameraControl.Set((CameraControlProperty)10, -160, CameraControlFlags.Manual);
            Console.WriteLine($"Property 10: value: {value}");

        }

    

        private void Form1_Load(object sender, EventArgs e)
        {
// Initialize the joystick position
    joystickPosition = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);

        }

        private void CONNECTED_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

    private void buttonSaveProfile_Click(object sender, EventArgs e)
{
    Console.WriteLine($"Inside buttonSaveProfile_Click. Profiles is {(Profiles == null ? "null" : "not null")}");

    if (comboDevice.SelectedItem == null)
    {
        Console.WriteLine("comboDevice.SelectedItem is null");
        return;
    }

    if (textPort.Text == null)
    {
        Console.WriteLine("textPort.Text is null");
        return;
    }

    if (Profiles == null)
    {
        Console.WriteLine("Profiles is null");
        return;
    }

    var profile = new Profile
    {
        DeviceId = comboDevice.SelectedItem.ToString(),
        Port = int.Parse(textPort.Text)
    };
    Profiles.Add(profile);
    SaveProfilesToFile();
    UpdateProfileList();
}
private void SaveProfilesToFile()
{
    var profilesJson = JsonConvert.SerializeObject(Profiles);
    File.WriteAllText("profiles.json", profilesJson);

    if (CameraPositions.Count > 0)
    {
        var positionsJson = JsonConvert.SerializeObject(CameraPositions);
        File.WriteAllText("positions.json", positionsJson);
    }
}


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }internal void SAVEPOS_Click(object sender, EventArgs e)
{
    var cameraControl = theDevice as IAMCameraControl;
    if (cameraControl == null) return;

    cameraControl.Get(CameraControlProperty.Pan, out int pan, out var flags);
    cameraControl.Get(CameraControlProperty.Tilt, out int tilt, out flags);
    cameraControl.Get(CameraControlProperty.Zoom, out int zoom, out flags);

    var position = new CameraPosition { Pan = pan, Tilt = tilt, Zoom = zoom };
    CameraPositions.Add(position);

    string positionName = $"POS #{CameraPositions.Count} (Pan: {pan}, Tilt: {tilt}, Zoom: {zoom})";
    POSBOX.Items.Add(positionName);
     SavePositionsToFile();
}private void SavePositionsToFile()
{
    var positionsJson = JsonConvert.SerializeObject(CameraPositions);
    File.WriteAllText("positions.json", positionsJson);
}
internal void DELPOS_Click(object sender, EventArgs e)
{
    int selectedIndex = POSBOX.SelectedIndex;
    if (selectedIndex != -1)
    {
        CameraPositions.RemoveAt(selectedIndex);
        POSBOX.Items.RemoveAt(selectedIndex);
    }
}
    }

    public class CameraControl
    {
        private IBaseFilter theDevice = null;
        private IAMCameraControl cameraControl = null;
        private int lastX = 0;
        private int lastY = 0;
        private int lastZoom = 0;
        private Form1 _parent = null;
        private OscReceiver  _listener = null;
        private OscSender oscSender;
        private System.Timers.Timer timer;
        private int lastPan, lastTilt;
        private IPEndPoint _clientEndPoint;
        Boolean isSetup = false;
       public CameraControl(int port, Form1 parent)
{
    _parent = parent;
 

    _listener = new OscReceiver(port);
    _listener.Connect();

    Task.Run(() =>
    {
        while (_listener.State != OscSocketState.Closed)
        {
            OscPacket packet = _listener.Receive();
            var messageReceived = (OscMessage)packet;
            receiveOSC(messageReceived);
              // Print the sender's IP and port to the console
        Console.WriteLine($"Received OSC message from {packet.Origin.Address}:{packet.Origin.Port}");
         
          // Initialize and update the OscSender port with the first incoming message port
            if (oscSender == null)
            {
                oscSender = new OscSender(IPAddress.Parse("127.0.0.1"), 0, packet.Origin.Port);
                oscSender.Connect();
            }

            // Start the timer after the first OSC message is received
            if (timer == null)
            {
                timer = new System.Timers.Timer(100); // Set interval 
                timer.Elapsed += CheckCameraPosition;
                timer.Start();
            }
        }
    });
}

        public void stop()
        {
            Console.WriteLine("Cleanup on isle 7");
            _listener.Close();
        }private void CheckCameraPosition(object sender, System.Timers.ElapsedEventArgs e)
{ 
    if (oscSender == null)
    {
        // Log an error message or throw an exception
        return;
    }
    cameraControl.Get(CameraControlProperty.Pan, out int pan, out var flags);
    cameraControl.Get(CameraControlProperty.Tilt, out int tilt, out flags);
    cameraControl.Get(CameraControlProperty.Zoom, out int zoom, out flags);

    if (pan != lastPan)
    {
        SendProperty(CameraControlProperty.Pan, "/Pan");
        lastPan = pan;
    }

    if (tilt != lastTilt)
    {
        SendProperty(CameraControlProperty.Tilt, "/Tilt");
        lastTilt = tilt;
    }

    if (zoom != lastZoom)
    {
        SendProperty(CameraControlProperty.Zoom, "/Zoom");
        lastZoom = zoom;
    }
}

      private void SendProperty(CameraControlProperty property, string oscAddress)
{
    cameraControl.Get(property, out int value, out var flags);
    var oscMessage = new OscMessage(oscAddress, value);
    oscSender.Send(oscMessage);
}


      private void receiveOSC(OscPacket packet)
{
    if (!isSetup)
            {
                object source = null;
                Guid iid = typeof(IBaseFilter).GUID;
                foreach (DsDevice device in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
                {
                    if (device.DevicePath.CompareTo(_parent.theDevicePath) == 0)
                    {
                        try
                        {
                            device.Mon.BindToObject(null, null, ref iid, out source);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"device failed: {ex}:");
                            //Console.WriteLine(ex.ToString());
                            continue;
                        }
                        break;
                    }
                }
                theDevice = (IBaseFilter)source;
                cameraControl = theDevice as IAMCameraControl;
                isSetup = true;
             }
    else if (cameraControl != null)
    {
          if (packet is OscMessage messageReceived)
    {  if (_listener == null)
    {
        // Log an error message or throw an exception
        return;
    }
        string address = messageReceived.Address;
        object[] args = messageReceived.ToArray();
    if (address == "/XY")
    {
        int valueX = (int)args[0];
        int valueY = ((int)args[1]) * -1;

                    if (lastX != valueX)
                        cameraControl.Set((CameraControlProperty)10, valueX, CameraControlFlags.Manual);
                    if (lastY != valueY)
                        cameraControl.Set((CameraControlProperty)11, valueY, CameraControlFlags.Manual);

                    lastX = valueX;
                    lastY = valueY;
                    _parent?.textLastMessage?.Invoke((MethodInvoker)delegate {
                        _parent.textLastMessage.Text = $"{address} {lastX} {lastY}";
                    });
                }
                else if (address == "/ZOOM")
                {
                     int valueZoom = (int)args[0];

                    if (lastZoom != valueZoom)
                        cameraControl.Set((CameraControlProperty)13, valueZoom, CameraControlFlags.Manual);

                    lastZoom = valueZoom;
                    _parent?.textLastMessage?.Invoke((MethodInvoker)delegate {
                        _parent.textLastMessage.Text = $"{address} {lastZoom}";
                    });

                }
                if (address == "/CENTER")
                {
                    cameraControl.GetRange(CameraControlProperty.Pan,
                        out int min, out int max, out int steppingDelta,
                        out int defaultValue, out var flags);
                    cameraControl.Set(CameraControlProperty.Pan, 0, CameraControlFlags.Manual);

                    cameraControl.GetRange(CameraControlProperty.Tilt,
                        out min, out max, out steppingDelta,
                        out defaultValue, out flags);
                    cameraControl.Set(CameraControlProperty.Tilt, defaultValue, CameraControlFlags.Manual);

                    cameraControl.GetRange(CameraControlProperty.Zoom,
                        out min, out max, out steppingDelta,
                        out defaultValue, out flags);
                    cameraControl.Set(CameraControlProperty.Zoom, defaultValue, CameraControlFlags.Manual);


                    _parent?.textLastMessage?.Invoke((MethodInvoker)delegate {
                        _parent.textLastMessage.Text = $"{address}";
                        
                    });
                }
               if (address == "/POSITION")
{
    int pan = (int)args[0];
    int tilt = (int)args[1];
    int zoom = (int)args[2];

    cameraControl.Set(CameraControlProperty.Pan, pan, CameraControlFlags.Manual);
    cameraControl.Set(CameraControlProperty.Tilt, tilt, CameraControlFlags.Manual);
    cameraControl.Set(CameraControlProperty.Zoom, zoom, CameraControlFlags.Manual);

    _parent?.textLastMessage?.Invoke((MethodInvoker)delegate {
        _parent.textLastMessage.Text = $"Pan: {pan}, Tilt: {tilt}, Zoom: {zoom}";
    });
}
                 if (address == "/INFO")
    {
        cameraControl.Get(CameraControlProperty.Pan, out int pan, out var flags);
        var oscMessagePan = new OscMessage("/Pan", pan);
        oscSender.Send(oscMessagePan);

        cameraControl.Get(CameraControlProperty.Tilt, out int tilt, out flags);
        var oscMessageTilt = new OscMessage("/Tilt", tilt);
        oscSender.Send(oscMessageTilt);

        cameraControl.Get(CameraControlProperty.Zoom, out int zoom, out flags);
        var oscMessageZoom = new OscMessage("/Zoom", zoom);
        oscSender.Send(oscMessageZoom);

        _parent?.textLastMessage?.Invoke((MethodInvoker)delegate {
            _parent.textLastMessage.Text = $"Pan: {pan}, Tilt: {tilt}, Zoom: {zoom}";
        });

    }  
    if (address.StartsWith("/TEST"))
{
    if (args.Length >= 2 && args[0] is int propertyIndex && args[1] is int value)
    {
        if (propertyIndex >= -1 && propertyIndex <= 21)
        {
            CameraControlProperty property = (CameraControlProperty)propertyIndex;
            cameraControl.Set(property, value, CameraControlFlags.Manual);
            Console.WriteLine($"Set Property: {property}, Value: {value}");
        }
    }
} if (address == "/POS" && args.Length >= 1 && args[0] is int positionIndex)
{
    _parent.Invoke((MethodInvoker)delegate {
        if (positionIndex == -1)
        {
            // Cycle to the next position
            int nextIndex = (_parent.POSBOX.SelectedIndex + 1) % _parent.CameraPositions.Count;
            _parent.POSBOX.SelectedIndex = nextIndex;
        }
        else if (positionIndex >= 0 && positionIndex < _parent.CameraPositions.Count)
        {
            // Select the position at the given index
            _parent.POSBOX.SelectedIndex = positionIndex;
        }
    });}
        else if (address == "/STORE")
        {
            // Save the current position
            _parent.Invoke((MethodInvoker)delegate {
                _parent.SAVEPOS_Click(null, EventArgs.Empty);
            });
        }
        else if (address == "/DELETE")
        {
            // Delete the current position
            _parent.Invoke((MethodInvoker)delegate {
                _parent.DELPOS_Click(null, EventArgs.Empty);
            });
        }
    

    
     }}}

    public void OSCThread()
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            // log errors
        }
    }public void cameraControlThread()
        {
            try
            {

            }
            catch (Exception ex)
            {
                // log errors
            }}
     


    
}
        
} 
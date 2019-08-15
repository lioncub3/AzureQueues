using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace ChefQueues
{
    public partial class Form1 : Form
    {
        private CloudStorageAccount storageAccount;
        private CloudQueueClient queueClient;
        private CloudQueue queue;
        public Form1()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference("queueorder");
            queue.CreateIfNotExists();

            InitializeComponent();

            CloudQueueMessage retrievedMessage = queue.PeekMessage();
            textBox1.Text = retrievedMessage.AsString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CloudQueueMessage retrievedMessage = queue.GetMessage();

            queue.DeleteMessage(retrievedMessage);

            textBox1.Text = queue.PeekMessage().AsString;
        }
    }
}

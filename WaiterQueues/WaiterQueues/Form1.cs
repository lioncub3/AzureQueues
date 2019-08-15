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

namespace WaiterQueues
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

            List<CloudQueueMessage> peekedMessages = queue.PeekMessages(20).ToList();
            
            InitializeComponent();

            foreach (var peekedMessage in peekedMessages)
            {
                orderListBox.Items.Add(peekedMessage.AsString);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            CloudQueueMessage message = new CloudQueueMessage(newOrderText.Text);
            await queue.AddMessageAsync(message);

            orderListBox.Items.Add(message.AsString);
        }
    }
}

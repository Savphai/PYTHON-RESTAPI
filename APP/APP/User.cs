using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Data.Common;
using System.Security.Policy;
using System.Runtime.CompilerServices;
using System.Data;

namespace APP
{
    class User
    {
        private string id;
        private string public_id;
        private string name;
        private string password;
        private string admin;
        private string url;
        public static string gettoken;
        private string access_token;
        private string message;
        public HttpClient client;
        public string Id { get => id; set => id = value; }
        public string Public_id { get => public_id; set => public_id = value; }
        public string Name { get => name; set => name = value; }
        public string Password { get => password; set => password = value; }
        public string Admin { get => admin; set => admin = value; }
        public string Url { get => url; set => url = value; }
        public string Access_token { get => access_token; set => access_token = value; }
        public string Message { get => message; set => message = value; }
        public DataGridViewRow dgv;
        public User()
        {
            client = new HttpClient();
            Url = "http://127.0.0.1:5000";
            client.BaseAddress = new Uri(Url);
            Access_token = gettoken;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gettoken);
        }
        public async Task<string> Login(Form form)
        {
            Dictionary<string, Object> payload = new Dictionary<string, Object>();
            payload.Add("name", Name);
            payload.Add("password", Password);
       
            var endpoint = "/login";
            var json = JsonConvert.SerializeObject(payload);
            Access_token = gettoken;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Access_token);
            HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(endpoint, data);
          //  MessageBox.Show(response.StatusCode.ToString());
            string message = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {

                List<User> userlist = JsonConvert.DeserializeObject<List<User>>(message);
                foreach (var item in userlist)
                    gettoken = item.Access_token.ToString();
                   
                User_FRM f = new User_FRM();
                f.Show();
                form.Hide();


            }
            else
            {
                List<User> userlist = JsonConvert.DeserializeObject<List<User>>(message);
                foreach (var item in userlist)
                    MessageBox.Show(item.Message.ToString());
            }




            return gettoken;
        }

        public async Task GetData(DataGridView dataGridView)
        {
            try

            {
                
                dataGridView.Rows.Clear();
                var endpoint = "/user";
                HttpResponseMessage response = await client.GetAsync(endpoint);
                var resp = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    List<User> userlist = JsonConvert.DeserializeObject<List<User>>(resp);
                    foreach (var item in userlist)
                    {
                        String[] record = { item.Id.ToString(), item.public_id.ToString(), item.Name.ToString(),
                            item.Password.ToString(), item.Admin.ToString() };
                        dataGridView.Rows.Add(record);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Get all user:\n" + ex.Message);
            }
        }
        public async Task Save()
        {

            try
            {
                Dictionary<string, Object> payload = new Dictionary<string, Object>();
                payload.Add("name", Name);
                payload.Add("password", Password);
                var endpoint = "/user";
                var json = JsonConvert.SerializeObject(payload);
                HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, data);
                string message = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    List<User> userlist = JsonConvert.DeserializeObject<List<User>>(message);
                    foreach (var item in userlist)
                        MessageBox.Show(item.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message);
            }
        }

        public async Task Delete(DataGridView dataGridView)
        {
            try
            {
                dgv = new DataGridViewRow();
                dgv = dataGridView.SelectedRows[0];
                Id = dgv.Cells[1].Value.ToString();
                var endpoint = "/user/" + Id + "";
                if (MessageBox.Show("Do you want to delete recode ?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    HttpResponseMessage response = await client.DeleteAsync(endpoint);
                    var message = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        List<User> userlist = JsonConvert.DeserializeObject<List<User>>(message);
                        foreach (var item in userlist)
                            MessageBox.Show(item.Message.ToString());
                        dataGridView.Rows.Remove(dgv);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message);
            }
        }
        public async Task PromoteUser(DataGridView dataGridView)
        {

            try
            {
                dgv = new DataGridViewRow();
                dgv = dataGridView.SelectedRows[0];
                Public_id = dgv.Cells[1].Value.ToString();
                Dictionary<string, Object> payload = new Dictionary<string, Object>();
                payload.Add("admin", Convert.ToBoolean(Admin));
                var endpoint = "/user/promote/" + Public_id;
                var json = JsonConvert.SerializeObject(payload);
                HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(endpoint, data);
                var message = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    List<User> userlist = JsonConvert.DeserializeObject<List<User>>(message);
                    foreach (var item in userlist)
                        MessageBox.Show(item.Message.ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Promote User:\n" + ex.Message);
            }
        }

        public void TrasferDataToTextbox(DataGridView dataGridView, TextBox txtName, TextBox txtPassword, RadioButton rTrue, RadioButton rFalse)
        {
            if (dataGridView.Rows.Count == 0)
            {
                return;
            }
            dgv = new DataGridViewRow();
            dgv = dataGridView.SelectedRows[0];
            txtName.Text = dgv.Cells[2].Value.ToString();
            txtPassword.Text = dgv.Cells[3].Value.ToString();
            if (dgv.Cells[4].Value.ToString() == "true")
            {
                rTrue.Checked = true;
            }
            else
            {
                rFalse.Checked = true;
            }


        }


    }
}

using Plugin.Toast;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Common;
using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Views.Account.UpdateEmail;
using SilupostMobileApp.Views.Account.UpdateEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateEmailPage : ContentPage
    {
        UpdateEmailViewModel viewModel;
        public UpdateEmailPage(UpdateEmailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Title = "Update Email";
            this.viewModel.ProgressDialog.Hide();
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                if (this.viewModel.IsEmailUpdateddSuccess)
                {
                    await this.Navigation.PopToRootAsync();
                    MessagingCenter.Send(this, "Logout");
                }
                else
                {
                    await this.Navigation.PopAsync();
                }
            });
            return true;
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<EmailRegistrationContentView>(this, "SubmitEmail", async (obj) =>
            {
                try
                {
                    this.viewModel.ProgressDialog.Show();
                    await this.viewModel.ResetVerification();
                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        try
                        {
                            var model = new UpdateSystemUserNameBindingModel()
                            {
                                SystemUserId = AppSettingsHelper.AppSettings.UserSettings.SystemUserId,
                                UserName = this.viewModel.Email.Value
                            };
                            var result = await this.viewModel.UpdateEmail(model);
                            if (result != null)
                            {
                                NavigationPage.SetHasNavigationBar(this, false);
                                this.viewModel.IsEmailUpdateddSuccess = true;
                                this.viewModel.ProgressDialog.Hide();
                            }
                            else
                            {
                                this.viewModel.ProgressDialog.Hide();
                                SilupostPopMessage.ShowToastMessage(SilupostMessage.SERVER_ERROR);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!ex.Message.ToLower().Contains("object"))
                                SilupostExceptionLogger.GetError(ex);
                            this.viewModel.ProgressDialog.Hide();
                            this.viewModel.IsExecuting = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    this.viewModel.IsExecuting = false;
                    if (!ex.Message.ToLower().Contains("object"))
                        SilupostExceptionLogger.GetError(ex);
                    this.viewModel.ProgressDialog.Hide();
                }
            });

            this.Content = new EmailRegistrationContentView(this.viewModel);
        }
    }
}
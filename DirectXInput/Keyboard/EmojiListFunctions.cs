﻿using ArnoldVinkCode;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static ArnoldVinkCode.AVFocus;
using static DirectXInput.AppVariables;
using static LibraryShared.SoundPlayer;

namespace DirectXInput.KeyboardCode
{
    partial class WindowKeyboard
    {
        //Show or hide the emoji popup
        async Task ShowHideEmojiListPopup()
        {
            try
            {
                if (!CheckTextPopupsOpen())
                {
                    await ShowEmojiPopup();
                }
                else
                {
                    await HideTextPopups();
                }
            }
            catch { }
        }

        //Show the emoji popup
        async Task ShowEmojiPopup()
        {
            //Play window open sound
            PlayInterfaceSound(vConfigurationDirectXInput, "PopupOpen", false, false);

            //Store keyboard focus button
            AVFocusDetailsSave(vFocusedButtonKeyboard, null);

            //Show the emoji menu
            border_EmojiListPopup.Visibility = Visibility.Visible;
            grid_Keyboard_Keys.IsEnabled = false;

            //Update the help bar
            textblock_LeftTriggerOff.Text = string.Empty;
            textblock_RightTriggerOff.Text = "Switch emoji";

            //Focus on popup button
            if (vFocusedButtonEmoji.FocusListBox == null)
            {
                AVFocusDetails focusListbox = new AVFocusDetails();
                focusListbox.FocusListBox = listbox_EmojiList;
                focusListbox.FocusIndex = vLastPopupListEmojiIndex;
                await AVFocusDetailsFocus(focusListbox, vInteropWindowHandle);
            }
            else
            {
                await AVFocusDetailsFocus(vFocusedButtonEmoji, vInteropWindowHandle);
            }
        }

        //Hide the emoji popup
        async Task HideEmojiPopup()
        {
            try
            {
                //Play window close sound
                PlayInterfaceSound(vConfigurationDirectXInput, "PopupClose", false, false);

                //Store open focus button
                AVFocusDetailsSave(vFocusedButtonEmoji, null);

                //Hide the emoji menu
                border_EmojiListPopup.Visibility = Visibility.Collapsed;
                grid_Keyboard_Keys.IsEnabled = true;
                vLastPopupListType = "Emoji";
                vLastPopupListEmojiIndex = listbox_EmojiList.SelectedIndex;

                //Update the help bar
                textblock_LeftTriggerOff.Text = "Caps";
                textblock_RightTriggerOff.Text = "Tab";

                //Focus on keyboard button
                if (vFocusedButtonKeyboard.FocusElement == null)
                {
                    await FocusElement(key_EmojiList, vInteropWindowHandle);
                }
                else
                {
                    await AVFocusDetailsFocus(vFocusedButtonKeyboard, vInteropWindowHandle);
                }
            }
            catch { }
        }

        //Switch the emoji type list by button
        async void SwitchEmojiTypeListButton(object sender)
        {
            try
            {
                //Update the emoji selected index
                UpdateSelectedIndexEmoji();

                //Background brush variable
                SolidColorBrush selectedBrush = (SolidColorBrush)Application.Current.Resources["ApplicationAccentDarkBrush"];
                SolidColorBrush defaultBrush = (SolidColorBrush)Application.Current.Resources["ApplicationAccentLightBrush"];

                //Reset background brush
                key_EmojiSmiley.Background = defaultBrush;
                key_EmojiSmiley.BorderBrush = defaultBrush;
                key_EmojiActivity.Background = defaultBrush;
                key_EmojiActivity.BorderBrush = defaultBrush;
                key_EmojiFood.Background = defaultBrush;
                key_EmojiFood.BorderBrush = defaultBrush;
                key_EmojiNature.Background = defaultBrush;
                key_EmojiNature.BorderBrush = defaultBrush;
                key_EmojiOther.Background = defaultBrush;
                key_EmojiOther.BorderBrush = defaultBrush;
                key_EmojiPeople.Background = defaultBrush;
                key_EmojiPeople.BorderBrush = defaultBrush;
                key_EmojiSymbol.Background = defaultBrush;
                key_EmojiSymbol.BorderBrush = defaultBrush;
                key_EmojiTravel.Background = defaultBrush;
                key_EmojiTravel.BorderBrush = defaultBrush;

                //Switch the emoji list
                int selectIndex = 0;
                if (sender == key_EmojiSmiley)
                {
                    listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListSmiley;
                    key_EmojiSmiley.Background = selectedBrush;
                    key_EmojiSmiley.BorderBrush = selectedBrush;
                    selectIndex = vDirectKeyboardEmojiIndexSmiley;
                }
                else if (sender == key_EmojiActivity)
                {
                    listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListActivity;
                    key_EmojiActivity.Background = selectedBrush;
                    key_EmojiActivity.BorderBrush = selectedBrush;
                    selectIndex = vDirectKeyboardEmojiIndexActivity;
                }
                else if (sender == key_EmojiFood)
                {
                    listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListFood;
                    key_EmojiFood.Background = selectedBrush;
                    key_EmojiFood.BorderBrush = selectedBrush;
                    selectIndex = vDirectKeyboardEmojiIndexFood;
                }
                else if (sender == key_EmojiNature)
                {
                    listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListNature;
                    key_EmojiNature.Background = selectedBrush;
                    key_EmojiNature.BorderBrush = selectedBrush;
                    selectIndex = vDirectKeyboardEmojiIndexNature;
                }
                else if (sender == key_EmojiOther)
                {
                    listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListOther;
                    key_EmojiOther.Background = selectedBrush;
                    key_EmojiOther.BorderBrush = selectedBrush;
                    selectIndex = vDirectKeyboardEmojiIndexOther;
                }
                else if (sender == key_EmojiPeople)
                {
                    listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListPeople;
                    key_EmojiPeople.Background = selectedBrush;
                    key_EmojiPeople.BorderBrush = selectedBrush;
                    selectIndex = vDirectKeyboardEmojiIndexPeople;
                }
                else if (sender == key_EmojiSymbol)
                {
                    listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListSymbol;
                    key_EmojiSymbol.Background = selectedBrush;
                    key_EmojiSymbol.BorderBrush = selectedBrush;
                    selectIndex = vDirectKeyboardEmojiIndexSymbol;
                }
                else if (sender == key_EmojiTravel)
                {
                    listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListTravel;
                    key_EmojiTravel.Background = selectedBrush;
                    key_EmojiTravel.BorderBrush = selectedBrush;
                    selectIndex = vDirectKeyboardEmojiIndexTravel;
                }

                //Focus on the current emoji
                AVFocusDetails focusListbox = new AVFocusDetails();
                focusListbox.FocusListBox = listbox_EmojiList;
                focusListbox.FocusIndex = selectIndex;
                await AVFocusDetailsFocus(focusListbox, vInteropWindowHandle);
            }
            catch { }
        }

        //Update the emoji selected index
        void UpdateSelectedIndexEmoji()
        {
            try
            {
                if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListSmiley)
                {
                    vDirectKeyboardEmojiIndexSmiley = listbox_EmojiList.SelectedIndex;
                }
                else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListActivity)
                {
                    vDirectKeyboardEmojiIndexActivity = listbox_EmojiList.SelectedIndex;
                }
                else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListFood)
                {
                    vDirectKeyboardEmojiIndexFood = listbox_EmojiList.SelectedIndex;
                }
                else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListNature)
                {
                    vDirectKeyboardEmojiIndexNature = listbox_EmojiList.SelectedIndex;
                }
                else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListOther)
                {
                    vDirectKeyboardEmojiIndexOther = listbox_EmojiList.SelectedIndex;
                }
                else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListPeople)
                {
                    vDirectKeyboardEmojiIndexPeople = listbox_EmojiList.SelectedIndex;
                }
                else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListSymbol)
                {
                    vDirectKeyboardEmojiIndexSymbol = listbox_EmojiList.SelectedIndex;
                }
                else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListTravel)
                {
                    vDirectKeyboardEmojiIndexTravel = listbox_EmojiList.SelectedIndex;
                }
            }
            catch { }
        }

        //Switch the emoji type list by trigger
        async Task SwitchEmojiTypeListTrigger(bool previous)
        {
            try
            {
                int selectIndex = 0;
                AVActions.DispatcherInvoke(delegate
                {
                    //Update the emoji selected index
                    UpdateSelectedIndexEmoji();

                    //Background brush variable
                    SolidColorBrush selectedBrush = (SolidColorBrush)Application.Current.Resources["ApplicationAccentDarkBrush"];
                    SolidColorBrush defaultBrush = (SolidColorBrush)Application.Current.Resources["ApplicationAccentLightBrush"];

                    //Reset background brush
                    key_EmojiSmiley.Background = defaultBrush;
                    key_EmojiSmiley.BorderBrush = defaultBrush;
                    key_EmojiActivity.Background = defaultBrush;
                    key_EmojiActivity.BorderBrush = defaultBrush;
                    key_EmojiFood.Background = defaultBrush;
                    key_EmojiFood.BorderBrush = defaultBrush;
                    key_EmojiNature.Background = defaultBrush;
                    key_EmojiNature.BorderBrush = defaultBrush;
                    key_EmojiOther.Background = defaultBrush;
                    key_EmojiOther.BorderBrush = defaultBrush;
                    key_EmojiPeople.Background = defaultBrush;
                    key_EmojiPeople.BorderBrush = defaultBrush;
                    key_EmojiSymbol.Background = defaultBrush;
                    key_EmojiSymbol.BorderBrush = defaultBrush;
                    key_EmojiTravel.Background = defaultBrush;
                    key_EmojiTravel.BorderBrush = defaultBrush;

                    //Switch the emoji list
                    if (!previous)
                    {
                        if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListSmiley)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListActivity;
                            key_EmojiActivity.Background = selectedBrush;
                            key_EmojiActivity.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexActivity;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListActivity)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListFood;
                            key_EmojiFood.Background = selectedBrush;
                            key_EmojiFood.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexFood;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListFood)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListNature;
                            key_EmojiNature.Background = selectedBrush;
                            key_EmojiNature.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexNature;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListNature)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListOther;
                            key_EmojiOther.Background = selectedBrush;
                            key_EmojiOther.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexOther;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListOther)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListPeople;
                            key_EmojiPeople.Background = selectedBrush;
                            key_EmojiPeople.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexPeople;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListPeople)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListSymbol;
                            key_EmojiSymbol.Background = selectedBrush;
                            key_EmojiSymbol.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexSymbol;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListSymbol)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListTravel;
                            key_EmojiTravel.Background = selectedBrush;
                            key_EmojiTravel.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexTravel;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListTravel)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListSmiley;
                            key_EmojiSmiley.Background = selectedBrush;
                            key_EmojiSmiley.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexSmiley;
                        }
                    }
                    else
                    {
                        if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListSmiley)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListTravel;
                            key_EmojiTravel.Background = selectedBrush;
                            key_EmojiTravel.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexTravel;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListTravel)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListSymbol;
                            key_EmojiSymbol.Background = selectedBrush;
                            key_EmojiSymbol.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexSymbol;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListSymbol)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListPeople;
                            key_EmojiPeople.Background = selectedBrush;
                            key_EmojiPeople.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexPeople;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListPeople)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListOther;
                            key_EmojiOther.Background = selectedBrush;
                            key_EmojiOther.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexOther;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListOther)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListNature;
                            key_EmojiNature.Background = selectedBrush;
                            key_EmojiNature.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexNature;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListNature)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListFood;
                            key_EmojiFood.Background = selectedBrush;
                            key_EmojiFood.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexFood;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListFood)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListActivity;
                            key_EmojiActivity.Background = selectedBrush;
                            key_EmojiActivity.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexActivity;
                        }
                        else if (listbox_EmojiList.ItemsSource == vDirectKeyboardEmojiListActivity)
                        {
                            listbox_EmojiList.ItemsSource = vDirectKeyboardEmojiListSmiley;
                            key_EmojiSmiley.Background = selectedBrush;
                            key_EmojiSmiley.BorderBrush = selectedBrush;
                            selectIndex = vDirectKeyboardEmojiIndexSmiley;
                        }
                    }
                });

                //Focus on the first emoji
                AVFocusDetails focusListbox = new AVFocusDetails();
                focusListbox.FocusListBox = listbox_EmojiList;
                focusListbox.FocusIndex = selectIndex;
                await AVFocusDetailsFocus(focusListbox, vInteropWindowHandle);
            }
            catch { }
        }

        //Switch emoji button
        void ButtonSwitchEmoji_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Space)
                {
                    SwitchEmojiTypeListButton(sender);
                }
            }
            catch { }
        }
        void ButtonSwitchEmoji_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SwitchEmojiTypeListButton(sender);
            }
            catch { }
        }

        //Handle emoji close
        async void ButtonCloseEmoji_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Space)
                {
                    await ShowHideEmojiListPopup();
                }
            }
            catch { }
        }
        async void ButtonCloseEmoji_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                await ShowHideEmojiListPopup();
            }
            catch { }
        }

        //Handle emoji click
        void listbox_EmojiList_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //Check if an actual ListBoxItem is clicked
                if (!AVFunctions.ListBoxItemClickCheck((DependencyObject)e.OriginalSource)) { return; }

                KeyTypeStringSenderShared(sender);
            }
            catch { }
        }

        void listbox_EmojiList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Space)
                {
                    KeyTypeStringSenderShared(sender);
                }
            }
            catch { }
        }
    }
}
#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made 
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using System.IO;
using System.Linq;
using System.Threading;
using OpenRA.FileFormats;
using OpenRA.FileFormats.Graphics;
using OpenRA.Widgets;

namespace OpenRA.Mods.Cnc.Widgets
{	
	public class CncInstallFromCDLogic : IWidgetDelegate
	{
		Widget panel;
		ProgressBarWidget progressBar;
		LabelWidget statusLabel;
		Action continueLoading;
		
		[ObjectCreator.UseCtor]
		public CncInstallFromCDLogic([ObjectCreator.Param] Widget widget,
		                       [ObjectCreator.Param] Action continueLoading)
		{
			this.continueLoading = continueLoading;
			panel = widget.GetWidget("INSTALL_FROMCD_PANEL");
			progressBar = panel.GetWidget<ProgressBarWidget>("PROGRESS_BAR");
			statusLabel = panel.GetWidget<LabelWidget>("STATUS_LABEL");
			
			var backButton = panel.GetWidget<ButtonWidget>("BACK_BUTTON");
			backButton.OnClick = Widget.CloseWindow;
			backButton.IsVisible = () => false;
			
			var retryButton = panel.GetWidget<ButtonWidget>("RETRY_BUTTON");
			retryButton.OnClick = PromptForCD;
			retryButton.IsVisible = () => false;
			
			// TODO: Search obvious places (platform dependent) for CD
			PromptForCD();
		}
		
		void PromptForCD()
		{
			if (Game.Settings.Graphics.Mode == WindowMode.Fullscreen)
			{
				statusLabel.GetText = () => "Error: Installing from Fullscreen mode is not supported";
				panel.GetWidget("BACK_BUTTON").IsVisible = () => true;
				return;
			}
			
			progressBar.SetIndeterminate(true);
			Game.Utilities.PromptFilepathAsync("Select CONQUER.MIX on the C&C CD", path => Game.RunAfterTick(() => Install(path)));
		}
		
		void Install(string path)
		{
			var dest = new string[] { Platform.SupportDir, "Content", "cnc" }.Aggregate(Path.Combine);
			var copyFiles = new string[] { "CONQUER.MIX", "DESERT.MIX",
					"GENERAL.MIX", "SCORES.MIX", "SOUNDS.MIX", "TEMPERAT.MIX", "WINTER.MIX"};
			
			var extractPackage = "INSTALL/SETUP.Z";
			var extractFiles = new string[] { "cclocal.mix", "speech.mix", "tempicnh.mix", "updatec.mix" };

			progressBar.SetIndeterminate(false);
			
			var installCounter = 0;
			var onProgress = (Action<string>)(s =>
			{
				progressBar.Percentage = installCounter*100/(copyFiles.Count() + extractFiles.Count());
				installCounter++;
				
				statusLabel.GetText = () => s;
			});
			
			var onError = (Action<string>)(s =>
			{
				Game.RunAfterTick(() => 
				{
					statusLabel.GetText = () => "Error: "+s;
					panel.GetWidget("RETRY_BUTTON").IsVisible = () => true;
					panel.GetWidget("BACK_BUTTON").IsVisible = () => true;
				});
			});
			
			string source;
			try 
			{
				source = Path.GetDirectoryName(path);
			}
			catch (ArgumentException)
			{
				onError("Invalid path selected");
				return;
			}
			
			var t = new Thread( _ =>
			{
				if (!InstallUtils.CopyFiles(source, copyFiles, dest, onProgress, onError))
				return;
			
				if (!InstallUtils.ExtractFromPackage(source, extractPackage, extractFiles, dest, onProgress, onError))
			    	return;
				
				Game.RunAfterTick(() =>
				{
					Widget.CloseWindow();
					continueLoading();
				});
			}) { IsBackground = true };
			t.Start();
		}
	}
}
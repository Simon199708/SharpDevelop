﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Drawing;
using ICSharpCode.Reports.Core.BaseClasses.Printing;
using ICSharpCode.Reports.Core.Interfaces;

namespace ICSharpCode.Reports.Core.Exporter
{
	/// <summary>
	/// Description of TableConverter.
	/// </summary>
	public class TableConverter:BaseConverter
	{

		private ITableContainer baseTable;
		
		public TableConverter(IDataNavigator dataNavigator,
		                      ExporterPage singlePage, ILayouter layouter ):base(dataNavigator,singlePage,layouter)
			
		{
		}
		
		
		public override ExporterCollection Convert (BaseReportItem parent,BaseReportItem item)
		{
			if (parent == null) {
				throw new ArgumentNullException("parent");
			}
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			
			ExporterCollection mylist = base.Convert(parent,item);
			this.baseTable = (BaseTableItem)item ;
			this.baseTable.Parent = parent;
			this.baseTable.DataNavigator = base.DataNavigator;
			return ConvertInternal(mylist);
		}
		
		
		private ExporterCollection ConvertInternal(ExporterCollection mylist)
		{
			
			Point currentPosition = new Point(PrintHelper.DrawingAreaRelativeToParent(this.baseTable.Parent,this.baseTable).Location.X,
			                                  base.SectionBounds.DetailStart.Y);
			
			int defaultLeftPos = currentPosition.X;
			
			Point dataAreaStart = new Point(baseTable.Items[0].Location.X,baseTable.Items[0].Location.Y + currentPosition.Y);
			
			ISimpleContainer headerContainer = null;
			
			
			
			this.baseTable.Items.SortByLocation();
			
			foreach (ISimpleContainer simpleContainer in this.baseTable.Items)
			{
				if (simpleContainer.Items.Count > 0) {
					simpleContainer.Location = new Point (simpleContainer.Location.X,simpleContainer.Location.Y);
					simpleContainer.Parent = (BaseReportItem)this.baseTable;
					base.SaveSize( new Size (simpleContainer.Size.Width,simpleContainer.Size.Height));
					// Header/FooterRow
					
					if (PrintHelper.IsTextOnlyRow(simpleContainer) ) {
						headerContainer = simpleContainer;
						currentPosition = BaseConverter.BaseConvert(mylist,headerContainer,defaultLeftPos,currentPosition);
					}
					
					else
					{
						// DataRegion
						base.SaveSize(simpleContainer.Size);
						do {
							//
							BaseSection section = this.baseTable.Parent as BaseSection;
							PrintHelper.AdjustSectionLocation(section);
							
//							base.FillRow(simpleContainer);
							
							StandardPrinter.EvaluateRow(base.Evaluator,mylist);
							
							base.PrepareContainerForConverting(null,simpleContainer);
							
							Rectangle pageBreakRect = PrintHelper.CalculatePageBreakRectangle((BaseReportItem)simpleContainer,currentPosition);
							
							if (PrintHelper.IsPageFull(pageBreakRect,base.SectionBounds))
							{
								base.BuildNewPage(mylist,section);
								
								currentPosition = BaseConverter.BaseConvert(mylist,headerContainer,
								                                            defaultLeftPos,
								                                            base.SectionBounds.ReportHeaderRectangle.Location);

							}
							
							currentPosition = BaseConverter.BaseConvert(mylist,simpleContainer,defaultLeftPos,currentPosition);
							
							simpleContainer.Size = base.RestoreSize;
						}
						while (base.DataNavigator.MoveNext());
						//Allway's reset the DataNavigator
						base.DataNavigator.Reset();
						base.DataNavigator.MoveNext();
						SectionBounds.ReportFooterRectangle =  new Rectangle(SectionBounds.ReportFooterRectangle.Left,
						                                                     currentPosition.Y,
						                                                     SectionBounds.ReportFooterRectangle.Width,
						                                                     SectionBounds.ReportFooterRectangle.Height);
					}
				}
			}
			return mylist;
		}
		
		
		
		private ExporterCollection old_ConvertInternal(ExporterCollection mylist)
		{
			
			Point currentPosition = new Point(PrintHelper.DrawingAreaRelativeToParent(this.baseTable.Parent,this.baseTable).Location.X,
			                                  base.SectionBounds.DetailStart.Y);
			
			int defaultLeftPos = currentPosition.X;
			
			Point dataAreaStart = new Point(baseTable.Items[0].Location.X,baseTable.Items[0].Location.Y + currentPosition.Y);
			
			ISimpleContainer headerContainer = null;
			
			
			
			this.baseTable.Items.SortByLocation();
			
			foreach (ISimpleContainer simpleContainer in this.baseTable.Items)
			{
				if (simpleContainer.Items.Count > 0) {
					simpleContainer.Location = new Point (simpleContainer.Location.X,simpleContainer.Location.Y);
					simpleContainer.Parent = (BaseReportItem)this.baseTable;
					base.SaveSize( new Size (simpleContainer.Size.Width,simpleContainer.Size.Height));
					// Header/FooterRow
					
					if (PrintHelper.IsTextOnlyRow(simpleContainer) ) {
						headerContainer = simpleContainer;
						currentPosition = BaseConverter.BaseConvert(mylist,headerContainer,defaultLeftPos,currentPosition);
					}
					
					else
					{
						// DataRegion
						base.SaveSize(simpleContainer.Size);
						do {
							//
							BaseSection section = this.baseTable.Parent as BaseSection;
							PrintHelper.AdjustSectionLocation(section);
							
//							base.FillRow(simpleContainer);
							
							StandardPrinter.EvaluateRow(base.Evaluator,mylist);
							
							base.PrepareContainerForConverting(null,simpleContainer);
							
							Rectangle pageBreakRect = PrintHelper.CalculatePageBreakRectangle((BaseReportItem)simpleContainer,currentPosition);
							
							if (PrintHelper.IsPageFull(pageBreakRect,base.SectionBounds))
							{
								base.BuildNewPage(mylist,section);
								
								currentPosition = BaseConverter.BaseConvert(mylist,headerContainer,
								                                            defaultLeftPos,
								                                            base.SectionBounds.ReportHeaderRectangle.Location);

							}
							
							currentPosition = BaseConverter.BaseConvert(mylist,simpleContainer,defaultLeftPos,currentPosition);
							
							simpleContainer.Size = base.RestoreSize;
						}
						while (base.DataNavigator.MoveNext());
						//Allway's reset the DataNavigator
						base.DataNavigator.Reset();
						base.DataNavigator.MoveNext();
						SectionBounds.ReportFooterRectangle =  new Rectangle(SectionBounds.ReportFooterRectangle.Left,
						                                                     currentPosition.Y,
						                                                     SectionBounds.ReportFooterRectangle.Width,
						                                                     SectionBounds.ReportFooterRectangle.Height);
					}
				}
			}
			return mylist;
		}
	}
}

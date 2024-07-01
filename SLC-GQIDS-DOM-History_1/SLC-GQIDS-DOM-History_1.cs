/*
****************************************************************************
*  Copyright (c) 2024,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

30/05/2024	1.0.1		AMA, Skyline	Initial version
18/06/2024	1.0.2		AMA, Skyline	Fixed exceptions when multiple sections or removed sections were present in the history
01/07/2024	1.0.3		AMA, Skyline	Return empty table, when no module or instance is given. This could be intentional, for example when no row is selected in a table
****************************************************************************
*/

// You can uncomment these directives to support newer features if your DataMiner system supports it.
// See the Github readme for more info on what these features do.
// https://github.com/SkylineCommunications/SLC-GQIDS-DOM-History

////#define DATAMINER_10_4_0

// Ignore Spelling: GQI
namespace Skyline.GQI.Sources.DOM.History
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Analytics.GenericInterface;
	using Skyline.GQI.Sources.DOM.History.Provider;

	[GQIMetaData(Name = "Get DOM History")]
	public class GetDOMHistory : IGQIDataSource, IGQIOnInit, IGQIInputArguments
	{
		private GQIDMS dms;
		private DataProvider dataProvider;
		private Guid domInstanceId;

		#region IGQIInputArguments
		private readonly GQIStringArgument domInstanceModuleArgs = new GQIStringArgument("DOM Module") { IsRequired = false, DefaultValue = String.Empty };
		private readonly GQIStringArgument domInstanceIdArgs = new GQIStringArgument("DOM Instance ID") { IsRequired = false, DefaultValue = String.Empty };

		public GQIArgument[] GetInputArguments()
		{
			return new GQIArgument[] { domInstanceModuleArgs, domInstanceIdArgs };
		}

		public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
		{
			if (!args.TryGetArgumentValue(domInstanceModuleArgs, out var domModule) ||
				String.IsNullOrEmpty(domModule))
			{
				// If no valid DOM Module ID is given return an empty table.
				dataProvider = null;
				return new OnArgumentsProcessedOutputArgs();
			}

			if (!args.TryGetArgumentValue(domInstanceIdArgs, out var rawDomInstanceId) ||
				!Guid.TryParse(rawDomInstanceId, out domInstanceId))
			{
				// If no valid DOM Instance ID is given return an empty table.
				dataProvider = null;
				return new OnArgumentsProcessedOutputArgs();
			}

			dataProvider = new DataProvider(dms, domModule);

			return new OnArgumentsProcessedOutputArgs();
		}
		#endregion

		#region IGQIDataSource

		private static readonly GQIColumn[] Columns = new GQIColumn[]
		{
			new GQIStringColumn("DOM History ID"),
			new GQIStringColumn("DOM Instance ID"),
			new GQIDateTimeColumn("Date of Change"),
			new GQIStringColumn("Changed By"),
			new GQIStringColumn("Type"),
			new GQIStringColumn("Section ID"),
			new GQIStringColumn("Section Definition ID"),
			new GQIStringColumn("Section Definition Name"),
			new GQIStringColumn("Field Descriptor ID"),
			new GQIStringColumn("Field Descriptor Name"),
			new GQIStringColumn("Old Value"),
			new GQIStringColumn("New Value"),
		};

		public GQIColumn[] GetColumns()
		{
			return Columns;
		}

		public GQIPage GetNextPage(GetNextPageInputArgs args)
		{
			if (dataProvider == null)
			{
				return new GQIPage(new GQIRow[0])
				{
					HasNextPage = false,
				};
			}

			var changes = dataProvider.GetHistoryForInstance(domInstanceId);

			var rows = changes.Select(change => new GQIRow(
				new[]
				{
					new GQICell { Value = change.DomHistoryId.ToString(), },
					new GQICell { Value = change.DomInstanceId, },
					new GQICell { Value = change.DateOfChange, },
					new GQICell { Value = change.ChangedBy, },
					new GQICell { Value = change.Type.ToString(), },
					new GQICell { Value = change.SectionId },
					new GQICell { Value = change.SectionDefinitionId },
					new GQICell { Value = change.SectionDefinitionName },
					new GQICell { Value = change.FieldDescriptorId },
					new GQICell { Value = change.FieldDescriptorName },
					new GQICell { Value = change.OldValue, },
					new GQICell { Value = change.NewValue, },
				})
#if DATAMINER_10_4
			{
				Metadata = new GenIfRowMetadata(new[] { new ObjectRefMetadata { Object = new DomInstanceId(change.Id), } }),
			}
#endif
			);

			return new GQIPage(rows.ToArray())
			{
				HasNextPage = false,
			};
		}

		#endregion

		#region IGQIOnInit

		public OnInitOutputArgs OnInit(OnInitInputArgs args)
		{
			dms = args.DMS;
			return new OnInitOutputArgs();
		}

		#endregion
	}
}

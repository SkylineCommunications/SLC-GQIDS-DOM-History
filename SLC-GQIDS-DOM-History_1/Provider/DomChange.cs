// Ignore Spelling: GQI

namespace Skyline.GQI.Sources.DOM.History.Provider
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;

	using Skyline.DataMiner.Net.Apps.History.DomInstances;
	using Skyline.DataMiner.Net.History;
	using Skyline.DataMiner.Net.Sections;

	public enum DomChangeType
	{
		[Description("Section")]
		SectionChange,

		[Description("Status")]
		StatusChange,
	}

	public class DomChange
	{
		public DomChange()
		{
		}

		public DomChange(Dictionary<Guid, KeyValuePair<SectionDefinition, FieldDescriptor>> fieldMap, HistoryChange history, DomSectionChange sectionChange, DomFieldValueChange fieldChange)
		{
			DomHistoryId = history.ID;
			DomInstanceId = Convert.ToString(history.SubjectId.ToAttachmentString());
			DateOfChange = history.Time;
			Type = DomChangeType.SectionChange;
			ChangedBy = history.FullUsername;
			SectionId = Convert.ToString(sectionChange.SectionId.Id);
			SectionDefinitionId = Convert.ToString(fieldMap[fieldChange.FieldDescriptorId.Id].Key.GetID().Id);
			SectionDefinitionName = fieldMap[fieldChange.FieldDescriptorId.Id].Key.GetName();
			FieldDescriptorId = Convert.ToString(fieldChange.FieldDescriptorId.Id);
			FieldDescriptorName = fieldMap[fieldChange.FieldDescriptorId.Id].Value.Name;
			OldValue = Convert.ToString(fieldChange.ValueBefore?.Value);
			NewValue = Convert.ToString(fieldChange.ValueAfter?.Value);
		}

		public DomChange(HistoryChange history, DomInstanceStatusChange change)
		{
			DomHistoryId = history.ID;
			DomInstanceId = Convert.ToString(history.SubjectId.ToAttachmentString());
			DateOfChange = history.Time;
			Type = DomChangeType.StatusChange;
			ChangedBy = history.FullUsername;
			OldValue = change.StatusIdBefore;
			NewValue = change.StatusIdAfter;
		}

		public Guid DomHistoryId { get; set; }

		public string DomInstanceId { get; set; }

		public DateTime DateOfChange { get; set; }

		public DomChangeType Type { get; set; }

		public string ChangedBy { get; set; }

		public string SectionId { get; set; } = "N/A";

		public string SectionDefinitionId { get; set; } = "N/A";

		public string SectionDefinitionName { get; set; } = "N/A";

		public string FieldDescriptorId { get; set; } = "N/A";

		public string FieldDescriptorName { get; set; } = "N/A";

		public string OldValue { get; set; }

		public string NewValue { get; set; }
	}
}

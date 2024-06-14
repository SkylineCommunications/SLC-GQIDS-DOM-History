// Ignore Spelling: GQI

namespace Skyline.GQI.Sources.DOM.History.Provider
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;

	using Skyline.DataMiner.Net.Apps.History.DomInstances;
	using Skyline.DataMiner.Net.History;

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

		public DomChange(
			List<DomFieldContainer> fieldMap,
			HistoryChange history,
			DomSectionChange sectionChange,
			DomFieldValueChange fieldChange)
		{
			var helperContainer = fieldMap.Find(x => x.Field.ID.Id == fieldChange.FieldDescriptorId.Id);
			if (helperContainer != null)
			{
				var sectionDefinition = helperContainer.SectionDefinition;
				var fieldDescriptor = helperContainer.Field;

				SectionDefinitionName = SectionDefinitionId = Convert.ToString(sectionDefinition.GetID().Id);
				SectionDefinitionName = sectionDefinition.GetName();
				FieldDescriptorName = fieldDescriptor.Name;
			}

			DomHistoryId = history.ID;
			DomInstanceId = Convert.ToString(history.SubjectId.ToAttachmentString());
			DateOfChange = history.Time;
			Type = DomChangeType.SectionChange;
			ChangedBy = history.FullUsername;
			SectionId = Convert.ToString(sectionChange.SectionId.Id);
			FieldDescriptorId = Convert.ToString(fieldChange.FieldDescriptorId.Id);
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

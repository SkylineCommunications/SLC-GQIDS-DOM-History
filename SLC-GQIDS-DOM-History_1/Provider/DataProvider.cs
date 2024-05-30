// Ignore Spelling: GQI dms

namespace Skyline.GQI.Sources.DOM.History.Provider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Analytics.GenericInterface;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.History.DomInstances;
	using Skyline.DataMiner.Net.History;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;

	public sealed class DataProvider
	{
		private readonly DomHelper helper;

		public DataProvider(GQIDMS dms, string module)
		{
			var connection = dms ?? throw new ArgumentNullException(nameof(dms));
			if (String.IsNullOrEmpty(module))
			{
				throw new ArgumentNullException(nameof(module));
			}

			helper = new DomHelper(connection.SendMessages, module);
		}

		public IEnumerable<DomChange> GetHistoryForInstance(Guid domInstanceId)
		{
			var instanceFilter = DomInstanceExposers.Id.Equal(domInstanceId);
			var instance = helper.DomInstances.Read(instanceFilter).SingleOrDefault();
			if (instance == null)
			{
				return new List<DomChange>();
			}

			var filter = HistoryChangeExposers.SubjectID.Equal(instance.ID.ToFileFriendlyString());
			var historyAll = helper.DomInstanceHistory.Read(filter);

			helper.StitchDomInstances(new List<DomInstance> { instance });
			var fieldMap = instance.Sections.SelectMany(section =>
			{
				var sectionDef = section.GetSectionDefinition();
				return sectionDef.GetAllFieldDescriptors().Select(field =>
					new { Field = field, Section = sectionDef });
			}).ToDictionary(key => key.Field.ID.Id, value => new KeyValuePair<SectionDefinition, FieldDescriptor>(value.Section, value.Field));

			var changes = new List<DomChange>();
			changes.AddRange(
				historyAll.SelectMany(history =>
					history.Changes.OfType<DomSectionChange>().SelectMany(sectionChange =>
						sectionChange.FieldValueChanges.Select(fieldChange =>
							new DomChange(fieldMap, history, sectionChange, fieldChange)))));

			changes.AddRange(
				historyAll.SelectMany(history =>
					history.Changes.OfType<DomInstanceStatusChange>().Select(change =>
						new DomChange(history, change))));

			return changes.OrderByDescending(x => x.DateOfChange);
		}
	}
}

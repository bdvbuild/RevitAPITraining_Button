using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Prism.Commands;
using RevitApiTrainingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitAPITraining_Button
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<Pipe> AllPipes { get; }
        public DelegateCommand PipesCount { get; }
        public List<Wall> AllWalls { get; }
        public DelegateCommand WallsVolume { get; }
        public List<FamilyInstance> AllDoors { get; }
        public DelegateCommand DoorsCount { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            AllPipes = AllElementSelection.GetAllPipes(commandData);
            PipesCount = new DelegateCommand(OnPipesCount);
            AllWalls = AllElementSelection.GetAllWalls(commandData);
            WallsVolume = new DelegateCommand(OnWallsVolume);
            AllDoors = AllElementSelection.GetAllDoors(commandData);
            DoorsCount = new DelegateCommand(OnDoorsCount);
        }


        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }

        private void OnPipesCount()
        {
            RaiseHideRequest();

            TaskDialog.Show("Сообщение", $"Количество труб: {AllPipes.Count}");

            RaiseShowRequest();
        }
        private void OnWallsVolume()
        {
            RaiseHideRequest();

            double volume = 0;
            foreach (var wall in AllWalls)
            {
                Parameter wallVol = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                double vol = UnitUtils.ConvertFromInternalUnits(wallVol.AsDouble(), UnitTypeId.CubicMeters);
                volume += vol;
            }
            TaskDialog.Show("Сообщение", $"Объем стен: {volume} м^3.{Environment.NewLine}Количество стен: {AllWalls.Count}");

            RaiseShowRequest();
        }
        private void OnDoorsCount()
        {
            RaiseHideRequest();

            TaskDialog.Show("Сообщение", $"Количество дверей: {AllDoors.Count}");

            RaiseShowRequest();
        }
    }
}

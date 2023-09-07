using Battleship_v2.Models;
using Battleship_v2.Services;
using Battleship_v2.Ships;
using Battleship_v2.Utility;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Battleship_v2.ViewModels
{
    public sealed class PlayingFieldViewModel : BaseViewModel
    {
        public PlayingFieldModel Model { get; set; }
        public PlayerType Owner { get; }
        private GameManagerService m_GameManager;

        public DataGrid DataGrid
        {
            get => m_DataGrid;
            set
            {
                m_DataGrid = value;
                NotifyPropertyChanged(nameof(Model));
            }
        }
        private DataGrid m_DataGrid;

        public DataTable Grid
        {
            get => m_Grid;
            set
            {
                m_Grid = value;
                NotifyPropertyChanged(nameof(Grid));
            }
        }
        private DataTable m_Grid;

        public DataGridCellInfo CurrentCell
        {
            get => m_CurrentCell;
            set
            {
                // If this is not the opponent grid terminate.
                if (Owner == PlayerType.You) return;

                // Set the value of the current cell to the backing field.
                m_CurrentCell = value;

                // Check if the Column or the Item is null, in that case termiante.
                if (m_CurrentCell.Column is null || m_CurrentCell.Item is null) return;

                // Call the processing Method in the Model.
                Model.GridCellClicked(m_CurrentCell.Column.DisplayIndex + 1, DataGrid.Items.IndexOf(((DataRowView)m_CurrentCell.Item)) + 1);
            }
        }
        private DataGridCellInfo m_CurrentCell;

        public List<Ship> Ships
        {
            get => m_Ships;
            set
            {
                m_Ships = value;
                NotifyPropertyChanged(nameof(Ships));
            }
        }
        private List<Ship> m_Ships;

        public PlayingFieldViewModel(PlayerType theOwner, List<ushort> theShipList)
        {
            Owner = theOwner;

            DataGrid = createDataGrid();

            Grid = createDataTable();

            // Setting the source
            DataGrid.BeginInit();
            DataGrid.ItemsSource = Grid.DefaultView;
            DataGrid.EndInit();

            Model = new PlayingFieldModel(this, theOwner == PlayerType.You, theShipList);
        }

        private DataGrid createDataGrid()
        {
            // Defining sizes.
            var starLength = new DataGridLength(1, DataGridLengthUnitType.Star);
            double aHeaderSize = 50;

            // Create a new DataGrid Object
            var aDataGrid = new DataGrid();

            // General config
            aDataGrid.AutoGenerateColumns = false;
            aDataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            aDataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            aDataGrid.SelectionMode = DataGridSelectionMode.Single;
            aDataGrid.SelectionUnit = DataGridSelectionUnit.Cell;

            // Binding for the currently selected cell.
            Binding aBinding = new Binding(nameof(CurrentCell));
            aBinding.Source = this;
            aBinding.Mode = BindingMode.OneWayToSource;
            aBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(aDataGrid, DataGrid.CurrentCellProperty, aBinding);

            // Header config
            aDataGrid.HeadersVisibility = DataGridHeadersVisibility.All;
            aDataGrid.RowHeaderWidth = aHeaderSize;
            aDataGrid.ColumnHeaderHeight = aHeaderSize;

            // Row configs
            aDataGrid.CanUserAddRows = false;
            aDataGrid.CanUserDeleteRows = false;
            aDataGrid.CanUserResizeRows = false;
            aDataGrid.RowHeight = starLength.DesiredValue;

            // Column configs
            aDataGrid.ColumnWidth = starLength;
            aDataGrid.CanUserReorderColumns = false;
            aDataGrid.CanUserResizeColumns = false;

            // Event Handlers
            aDataGrid.LoadingRow += DataGrid_LoadingRow;

            // Add DataColumns to the DataGrid
            for (char aLetter = 'A'; aLetter <= 'J'; aLetter++)
            {
                // Creating a new Image element.
                FrameworkElementFactory image = new FrameworkElementFactory(typeof(Image));
                // Set the new Binding
                image.SetBinding(Image.SourceProperty, new Binding($"{aLetter}"));

                // Create a new DataGridTemplateColumn
                DataGridColumn column = new DataGridTemplateColumn
                {
                    CellTemplate = new DataTemplate() { VisualTree = image },
                    Header = aLetter,
                };
                // Add the column
                aDataGrid.Columns.Add(column);
            }

            return aDataGrid;
        }

        private static DataTable createDataTable(string theDefaultValue = Tile.Water)
        {
            var aTable = new DataTable();

            for (char aLetter = 'A'; aLetter <= 'J'; aLetter++)
            {
                DataColumn col = new DataColumn($"{aLetter}");
                col.DefaultValue = theDefaultValue;
                aTable.Columns.Add(col);
            }

            for (int i = 0; i < 10; i++)
            {
                DataRow row = aTable.NewRow();
                aTable.Rows.Add(row);
            }

            return aTable;
        }

        public void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}

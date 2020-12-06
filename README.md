# Blazor.DataGrid

A generic Blazor Grid for displaying data, written from scratch with no JS.

Primary Features : 
<ul>
<li>Data filtering</li>
<li>Data editing</li>
<li>Multi-sort</li>
<li>Multi level grouping</li>
<li>Multi record selection (Shift + Click select all to clicked,Ctrl + Click add clicked to selection)</li>
</ul>

You can see a live demo of the examples here https://blazor.adventurouscoding.com/gridpage

Most basic Grid setup

```c#
<Grid NumberOfRecordsToDisplay="NumberOfRecordsToDisplay" GridAdapter="new GridOfflineAdapter<Record>(Record.GetRandomRecords(50000))">
    <GridColumns>
        <GridColumn Field="@nameof(Record.Id)" TextAlign="GridColumn.TextAlignOption.Right"/>
        <GridColumn Field="@nameof(Record.FirstName)"/>
        <GridColumn Field="@nameof(Record.LastName)"/>
        <GridColumn Field="@nameof(Record.Salary)"/>
        <GridColumn Field="@nameof(Record.IsRemote)"/>
    </GridColumns>
</Grid>
```

GridAdapter expects an implementation of IGridAdapter<T>, in this case an GridOfflineAdapter<Record> which works with data offline for demonstration purposes.
Each GridColumn expects at least the Field parameter.

Fully featured Grid setup

```c#
<Grid NumberOfRecordsToDisplay="NumberOfRecordsToDisplay" GridAdapter="new GridOfflineAdapter<Record>(Record.GetRandomRecords(50000))"
     HasDeleteButtons="true"
      HasEditButtons="true"
      MultipleOrdering="true">
    <GridFilters>
        <DefaultGridFilters TRecord="Record">
            <ExtraFilters>
                <Filter DisplayText="" Key="LastName" Type="typeof(string)" FilteringType="FilteringType.Contains"></Filter>
                <Filter Key="Id" DisplayText="Id greater or equal" Type="typeof(int)" FilteringType="FilteringType.GreaterThanOrEqual"></Filter>
                <Filter Key="Id" DisplayText="Id less or equal" Type="typeof(int)" FilteringType="FilteringType.LessThanOrEqual"></Filter>
                <Filter Key="Salary" DisplayText="Salary greater or equal" Type="typeof(int)" FilteringType="FilteringType.GreaterThanOrEqual"></Filter>
                <Filter Key="Salary" DisplayText="Salary less or equal" Type="typeof(int)" FilteringType="FilteringType.LessThanOrEqual"></Filter>
            </ExtraFilters>
        </DefaultGridFilters>
    </GridFilters>
    <GridColumns>
        <GridColumn Field="@nameof(Record.Id)" IsFilter="true" FilterDisplayText="Id equals" ValueType="typeof(int)" FilteringType="FilteringType.Equals" TextAlign="GridColumn.TextAlignOption.Right"></GridColumn>
        <GridColumn Field="@nameof(Record.FirstName)" IsFilter="true" FilteringType="FilteringType.Contains"></GridColumn>
        <GridColumn Field="@nameof(Record.LastName)"></GridColumn>
        <GridColumn Field="@nameof(Record.Salary)"></GridColumn>
        <GridColumn Field="@nameof(Record.IsRemote)" IsFilter="true" ValueType="typeof(bool)" FilterDisplayText="Not remote" FilteringType="FilteringType.NotEquals"></GridColumn>
    </GridColumns>
</Grid>
```

The grid now has delete and edit buttons enabled, multi-ordering is enabled and also we have filtering.
Filtering can be applied into the column declaration, but if we need more than 1 filter for a specific column, we can specify it in the extra filters compoment.

Filtering types are Contains, Equals, GreaterThan, GreaterThanOrEqual, LessThan, LessThanOrEqual, NotEquals.

DisplayText is the text of the label,if not specified the Key value will be shown instead.
Key is the name of the field that the filter will apply to.

If you wish to try it, all you need to do is implement your own IGridAdapter or extend the OfflineAdapter to suit your needs.


By no means this is a production Component for you to include in a commercial project. 
While it offers some decent functionality, it's an unfinished project and it might produce unexpected results.

# WolftechAPI
This is a RESTful API built using ASP .NET Core.

### Functionality
Upon a requst it is performing the following task:

1.  **Read file from disk** 
  The API is impemented expecting a .csv file-format. The file contains comma separated values with information about a company's departments and child departments in the following format:
  ```
  OID,Title,Color,DepartmentParent_OID
  1,US News,#F52612,
  2,Crime + Justice,#F52612,1
  3,Energy + Environment,#F52612,1
  4,Extreme Weather,#F52612,1
  5,Space + Science,#F52612,1
  6,International News,#EB5F25,
  7,Africa,#EB5F25,6
  8,Americas,#EB5F25,6
  9,Asia,#EB5F25,6
  10,Europe,#EB5F25,6
  ```
2.  **Parse data**
  The response data from the API is a JSON containing the department hierarchy structure.The response supports any depth in the hierarchy.  For each node it also gives a count of the descendants. This number includes not only the number of children, but the number of children, the childrens children and so on.<br/>
  Format should be like the following for the preceding dataset:
  ```
  [
    {
    "Oid": 1,
    "Title": "US News",
    "NumDecendants": 4,
    "Color": "#F52612",
    "Departments": [
        {
          "Oid": 2,
          "Title": "Crime + Justice",
          "NumDecendants": 0,
          "Color": "#F52612",
          "Departments": []
        },
        {
          "Oid": 3,
          "Title": "Energy + Environment",
          "NumDecendants": 0,
          "Color": "#F52612",
          "Departments": []
        },
        {
          "Oid": 4,
          "Title": "Extreme Weather",
          "NumDecendants": 0,
          "Color": "#F52612",
          "Departments": []
        },
        {
          "Oid": 5,
          "Title": "Space + Science",
          "NumDecendants": 0,
          "Color": "#F52612",
          "Departments": []
        }
      ]
    },
    {
      "Oid": 6,
      "Title": "International News",
      "NumDecendants": 4,
      "Color": "#EB5F25",
      "Departments": [
        {
          "Oid": 7,
          "Title": "Africa",
          "NumDecendants": 0,
          "Color": "#EB5F25",
          "Departments": []
        },
        {
          "Oid": 8,
          "Title": "Americas",
          "NumDecendants": 0,
          "Color": "#EB5F25",
          "Departments": []
        },
        {
          "Oid": 9,
          "Title": "Asia",
          "NumDecendants": 0,
          "Color": "#EB5F25",
          "Departments": []
        },
        {
          "Oid": 10,
          "Title": "Europe",
          "NumDecendants": 0,
          "Color": "#EB5F25",
          "Departments": []
        }
      ]
    }
  ]
  ```
3. **Response**
  The response data is a JSON in the preceding format. If the data loading or parsing fails a BadRequest response is given with an error message.

### Test
The project also includes unit testing. These are implemented using the [xUnit](https://xunit.net/) tool and Microsoft VisualStudio's test tools for unit testing.

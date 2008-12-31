using System;
namespace Theminds {
  [AttributeUsage(AttributeTargets.Class)]
  public class DesiresAppControlsAttribute : Attribute { }
  public class DesiresTestingAttribute : Attribute { }
  public class DesiresTestingWithMockAppAttribute : Attribute { }
}
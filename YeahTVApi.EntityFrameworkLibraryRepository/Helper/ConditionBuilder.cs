using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace YeahTVApiLibrary.EntityFrameworkRepository.Helper
{
    internal class ConditionBuilder : ExpressionVisitor
    {
        private List<object> m_arguments;
        private Stack<string> m_conditionParts;

        public string Condition { get; private set; }
        public object[] Arguments { get; private set; }

        public void Build(Expression expression)
        {
            PartialEvaluator evaluator = new PartialEvaluator();
            Expression evaluatedExpression = evaluator.Eval(expression);

            this.m_arguments = new List<object>();
            this.m_conditionParts = new Stack<string>();

            this.Visit(evaluatedExpression);

            this.Arguments = this.m_arguments.ToArray();
            this.Condition = this.m_conditionParts.Count > 0 ? this.m_conditionParts.Pop() : null;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b == null) return b;

            string opr;
            switch (b.NodeType)
            {
                case ExpressionType.Equal:
                    opr = "=";
                    break;
                case ExpressionType.NotEqual:
                    opr = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    opr = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    opr = ">=";
                    break;
                case ExpressionType.LessThan:
                    opr = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    opr = "<=";
                    break;
                case ExpressionType.AndAlso:
                    opr = "AND";
                    break;
                case ExpressionType.OrElse:
                    opr = "OR";
                    break;
                case ExpressionType.Add:
                    opr = "+";
                    break;
                case ExpressionType.Subtract:
                    opr = "-";
                    break;
                case ExpressionType.Multiply:
                    opr = "*";
                    break;
                case ExpressionType.Divide:
                    opr = "/";
                    break;
                default:
                    throw new NotSupportedException(b.NodeType + "is not supported.");
            }
            this.Visit(b.Left);
            this.Visit(b.Right);
            string right = this.m_conditionParts.Pop();
            string left = this.m_conditionParts.Pop();
            string condition = String.Format("({0} {1} {2})", left, opr, right);
            this.m_conditionParts.Push(condition);
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c == null) return c;
            this.m_arguments.Add(c.Value);
            this.m_conditionParts.Push(String.Format("{{{0}}}", this.m_arguments.Count - 1));
            return c;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m == null) return m;
            PropertyInfo propertyInfo = m.Member as PropertyInfo;
            if (propertyInfo == null) return m;
            this.m_conditionParts.Push(String.Format("`{0}`", propertyInfo.Name));
            return m;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m == null) return m;

            base.VisitMethodCall(m);

            var opr = "";
            var argumentString = "";
            var condition = "";
            var left = this.m_conditionParts.Pop();
            var right = this.m_conditionParts.Pop();
            switch (m.Method.Name)
            {
                case "Contains":
                    opr = "IN";
                    var argumentIndex = int.Parse(right.Replace("{", "").Replace("}", ""));

                    var i = argumentIndex;
                    var argumentList = this.m_arguments[argumentIndex] as List<string>;
                    argumentString = string.Join(",", argumentList.Select(c => "{" + i++ + "}"));

                    var restArguments = this.m_arguments.Count > i ? this.m_arguments.GetRange(argumentIndex + 1, this.m_arguments.Count) : new List<object>();

                    this.m_arguments.RemoveRange(argumentIndex, this.m_arguments.Count);
                    this.m_arguments.AddRange(argumentList.Union(restArguments));
                    condition = String.Format("({0} {1} ({2}))", left, opr, argumentString);
                    break;

                case "Equals":
                    opr = "=";
                    condition = String.Format("({0} {1} {2})", right, opr, left);
                    break;

                default:
                    throw new NotSupportedException(m.Method.Name + "is not supported.");
            }

            this.m_conditionParts.Push(condition);
            
            return m;
        }

    }

   

    public class Command
    {
        public string Text { get; set; }
        public object[] args { get; set; }
    }

    public static class Operate 
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <typeparam name="IEntity"></typeparam>
        /// <returns></returns>
        public static string GetTableName<IEntity>()
        {
            Type type = typeof(IEntity);
            var tableAttribute = type.GetCustomAttributes(false).OfType<System.ComponentModel.DataAnnotations.Schema.TableAttribute>().FirstOrDefault();
            string table = tableAttribute == null ? type.Name : tableAttribute.Name;
            if (table.Length > 2 && table.StartsWith("DB", StringComparison.CurrentCultureIgnoreCase))
            {
                table = table.Substring(2, table.Length - 2);
            }

            return table;
        }

        public static string GetEntityDBName<IEntity>(string columnName)
        {
            var name = columnName;

            if (!name.Contains("Id"))
                return name;

            var type = typeof(IEntity);
            var tablName = type.Name;

            if (tablName == "CoreSysHotelSencond")
            {
                if (name == "Id")
                { 
                    name = "HotelId"; 
                }
                else
                {
                    name = name.Replace("Id", "HotelId");
                }
            }

            return name;
        }
    }
}

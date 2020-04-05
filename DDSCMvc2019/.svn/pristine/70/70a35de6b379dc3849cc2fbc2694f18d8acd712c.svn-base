using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.SYS;
using System.Xml;
using CommonLibrary.DBA;
using log4net;
using System.Configuration;
using System.Data;

namespace CommonLibrary.Login
{
    public class DDSCDefaultLogin : ILoginProcess
    {
        private static ILog logger = LogManager.GetLogger(typeof(DDSCDefaultLogin));
        private DBASqlLog g_dba = new DBASqlLog(ConfigurationManager.ConnectionStrings["DDSCConnection"].ConnectionString);


        public UserData Login(LoginViewModel p_loginVM)
        {
            UserData p_userData = new UserData();
            string m_sql = @"SELECT  u.user_uuid, u.user_id, u.user_name, u.org_uuid, u.org_id, u.role_id, u.role_uuid, u.pwd_status,
                                    (SELECT dbo.fn_DecryptByPassPhrasePwd(CONVERT(VARBINARY(100), u.pwd, 2))) AS pwd,
                                    (SELECT dbo.fn_DecryptByPassPhrasePwd(CONVERT(VARBINARY(100), u.pwd2, 2))) AS pwd2,
                                    u.pwd3, u.pwd4, u.pwd_modify_date, u.user_mail, u.tel, u.user_fax, u.user_mobil, u.status_flag, u.retry, u.Salt,
                                    ISNULL(o.org_name, u.org_id) AS org_name,
                                    ISNULL(o.org_type, '') AS org_type,
                                    (SELECT var_char01
                                     FROM   ZT_SysCodeInfo
                                     WHERE  cate = 'PWD_PARM' AND code_id = 'PWD_PERIOD') AS pwd_limit,
                                    c.code_name AS role_name
                            FROM    ZT_AaUser AS u
                                    LEFT JOIN dbo.ZT_AaOrg AS o ON u.org_id = o.org_id
                                    LEFT JOIN dbo.ZT_SysCodeInfo c ON u.role_uuid = c.code_uuid
                            WHERE   u.user_id = @user_id AND u.status_flag <> 'D' ";
            try
            {
                List<DBParameter> m_paraList = new List<DBParameter>();
                m_paraList.Add(new DBParameter("@user_id", p_loginVM.LoginId));
                if (p_loginVM.TMId != null)
                {
                    //外網
                    m_sql += @" and u.org_id=@org_id";
                    m_paraList.Add(new DBParameter("@org_id", p_loginVM.TMId));
                }
                else
                {
                    //內網
                    m_sql += @" and u.org_id=@org_id";
                    m_paraList.Add(new DBParameter("@org_id", "12358067"));
                }
                DataTable m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                if (m_dataTable.Rows.Count > 0)
                {
                    AaUser m_user = new AaUser();
                    m_user.user_uuid = m_dataTable.Rows[0]["user_uuid"] == DBNull.Value ? Guid.Empty : (Guid)m_dataTable.Rows[0]["user_uuid"];
                    m_user.user_id = m_dataTable.Rows[0]["user_id"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["user_id"];
                    m_user.user_name = m_dataTable.Rows[0]["user_name"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["user_name"];
                    m_user.org_uuid = m_dataTable.Rows[0]["org_uuid"] == DBNull.Value ? Guid.Empty : (Guid)m_dataTable.Rows[0]["org_uuid"];
                    m_user.org_id = m_dataTable.Rows[0]["org_id"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["org_id"];
                    m_user.org_name = m_dataTable.Rows[0]["org_name"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["org_name"];
                    m_user.org_type = m_dataTable.Rows[0]["org_type"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["org_type"];
                    m_user.role_id = m_dataTable.Rows[0]["role_id"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["role_id"];
                    m_user.role_uuid = m_dataTable.Rows[0]["role_uuid"] == DBNull.Value ? Guid.Empty : (Guid)m_dataTable.Rows[0]["role_uuid"];
                    m_user.role_name = m_dataTable.Rows[0]["role_name"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["role_name"];
                    m_user.pwd_status = m_dataTable.Rows[0]["pwd_status"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["pwd_status"];
                    m_user.pwd = m_dataTable.Rows[0]["pwd"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["pwd"];
                    m_user.pwd2 = m_dataTable.Rows[0]["pwd2"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["pwd2"];
                    m_user.pwd3 = m_dataTable.Rows[0]["pwd3"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["pwd3"];
                    m_user.pwd4 = m_dataTable.Rows[0]["pwd4"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["pwd4"];
                    m_user.pwd_modify_date = m_dataTable.Rows[0]["pwd_modify_date"] == DBNull.Value ? DateTime.MinValue : (DateTime)m_dataTable.Rows[0]["pwd_modify_date"];
                    m_user.user_mail = m_dataTable.Rows[0]["user_mail"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["user_mail"];
                    m_user.tel = m_dataTable.Rows[0]["tel"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["tel"];
                    m_user.user_fax = m_dataTable.Rows[0]["user_fax"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["user_fax"];
                    m_user.user_mobil = m_dataTable.Rows[0]["user_mobil"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["user_mobil"];
                    m_user.status_flag = m_dataTable.Rows[0]["status_flag"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["status_flag"];
                    m_user.retry = m_dataTable.Rows[0]["retry"] == DBNull.Value ? 0 : Convert.ToInt32(m_dataTable.Rows[0]["retry"]);
                    m_user.Salt = m_dataTable.Rows[0]["Salt"] == DBNull.Value ? "" : (string)m_dataTable.Rows[0]["Salt"];
                    //密碼到期日 (For 外網使用者)
                    m_user.pwd_limit = m_dataTable.Rows[0]["pwd_limit"] == DBNull.Value ? 0 : Convert.ToInt32(m_dataTable.Rows[0]["pwd_limit"]);
                    m_user.pwd_limit_date = m_user.pwd_modify_date.AddMonths(m_user.pwd_limit);

                    //目前無系統切換, 暫時直接查詢 ZT_SysGroup
                    //                    m_sql = @"SELECT g.* FROM ZT_AaUserGroup AS u 
                    //JOIN ZT_AaGroup AS g ON g.group_uuid = u.group_uuid 
                    //WHERE u.user_uuid = @user_uuid";
                    m_sql = @"SELECT * FROM ZT_SysGroup WHERE group_id=@group_id";
                    m_paraList.Clear();
                    //m_paraList.Add(new DBParameter("@user_uuid", m_user.user_uuid));
                    m_paraList.Add(new DBParameter("@group_id", m_user.role_id));
                    m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                    List<AaGroup> m_groupList = new List<AaGroup>();
                    for (int i = 0; i < m_dataTable.Rows.Count; i++)
                    {
                        AaGroup m_role = new AaGroup();
                        m_role.group_uuid = m_dataTable.Rows[i]["group_uuid"] == DBNull.Value ? Guid.Empty : (Guid)m_dataTable.Rows[i]["group_uuid"];
                        //m_role.system_uuid = (Guid)m_dataTable.Rows[i]["system_uuid"];
                        m_role.system_id = m_dataTable.Rows[i]["system_id"] == DBNull.Value ? "" : (string)m_dataTable.Rows[i]["system_id"];
                        m_role.org_id = m_dataTable.Rows[i]["org_id"] == DBNull.Value ? "" : (string)m_dataTable.Rows[i]["org_id"];
                        m_role.group_id = m_dataTable.Rows[i]["group_id"] == DBNull.Value ? "" : (string)m_dataTable.Rows[i]["group_id"];
                        m_role.group_name = m_dataTable.Rows[i]["group_name"] == DBNull.Value ? "" : (string)m_dataTable.Rows[i]["group_name"];
                        m_role.admin_group = m_dataTable.Rows[i]["admin_group"] == DBNull.Value ? "" : (string)m_dataTable.Rows[i]["admin_group"];
                        m_role.status_flag = m_dataTable.Rows[i]["status_flag"] == DBNull.Value ? "" : (string)m_dataTable.Rows[i]["status_flag"];

                        m_groupList.Add(m_role);
                    }

                    if (m_groupList.Count > 0)
                    {
                        m_sql = @"SELECT p.*, p2.func_id AS SubProgramId, p2.func_name AS SubProgramName, p3.func_id AS RootProgramId, p3.func_name AS RootProgramName 
FROM ZT_SysProgram AS p 
JOIN ZT_SysGroupProgram AS g ON p.func_uuid = g.func_uuid 
JOIN ZT_SysGroup AS u ON u.group_uuid = g.group_uuid 
JOIN ZT_SysProgram AS p2 ON p2.func_uuid = p.super_uuid
JOIN ZT_SysProgram AS p3 ON p3.func_uuid = p2.super_uuid 
WHERE p.func_type='F' AND u.group_id IN ({0})
ORDER BY p3.seq_no,p2.seq_no,p.seq_no";
                        StringBuilder m_sb = new StringBuilder();
                        for (int i = 0; i < m_groupList.Count; i++)
                        {
                            if (i != m_groupList.Count - 1)
                            {
                                m_sb.Append(string.Format("@group_id{0},", i));
                            }
                            else
                            {
                                m_sb.Append(string.Format("@group_id{0}", i));
                            }
                        }
                        m_sql = string.Format(m_sql, m_sb.ToString());

                        m_paraList.Clear();
                        for (int i = 0; i < m_groupList.Count; i++)
                        {
                            m_paraList.Add(new DBParameter("@group_id" + i, m_groupList[i].group_id.Trim()));
                        }
                        m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                        XmlDocument p_xml = new XmlDocument();
                        XmlElement p_rootNode = p_xml.CreateElement("MAIN");
                        p_xml.AppendChild(p_rootNode);
                        for (int i = 0; i < m_dataTable.Rows.Count; i++)
                        {
                            string p_rootName = ((string)m_dataTable.Rows[i]["RootProgramId"]).Trim();
                            string p_subName = ((string)m_dataTable.Rows[i]["SubProgramId"]).Trim();
                            string p_proId = ((string)m_dataTable.Rows[i]["func_id"]).Trim();
                            if (p_rootName == "MAIN")
                            {
                                XmlElement p_node = (XmlElement)p_rootNode.SelectSingleNode(p_subName);
                                if (p_node == null)
                                {
                                    p_node = p_xml.CreateElement(p_subName);
                                    p_node.SetAttribute("Name", (string)m_dataTable.Rows[i]["SubProgramName"]);
                                    p_rootNode.AppendChild(p_node);
                                }
                                XmlElement p_proNode = p_xml.CreateElement(p_proId);
                                p_proNode.SetAttribute("Name", (string)m_dataTable.Rows[i]["func_name"]);
                                if (m_dataTable.Rows[i]["func_url"] != DBNull.Value)
                                {
                                    p_proNode.SetAttribute("Link", (string)m_dataTable.Rows[i]["func_url"]);
                                }
                                p_node.AppendChild(p_proNode);
                            }
                            else
                            {
                                ////選單(M)不存在則不加入程式(F)
                                //DataRow[] m_menuRow = m_dataTable.Select(string.Format("func_uuid='{0}'", (Guid)m_dataTable.Rows[i]["super_uuid"]));
                                //if(m_menuRow.Length == 0)
                                //{
                                //    continue;
                                //}

                                XmlElement p_rootProNode = (XmlElement)p_rootNode.SelectSingleNode(p_rootName);
                                if (p_rootProNode == null)
                                {
                                    p_rootProNode = p_xml.CreateElement(p_rootName);
                                    p_rootProNode.SetAttribute("Name", (string)m_dataTable.Rows[i]["RootProgramName"]);
                                    p_rootProNode.SetAttribute("Link", (string)m_dataTable.Rows[i]["func_url"]);
                                    p_rootNode.AppendChild(p_rootProNode);
                                }
                                XmlElement p_subProNode = (XmlElement)p_rootProNode.SelectSingleNode(p_subName);
                                if (p_subProNode == null)
                                {
                                    p_subProNode = p_xml.CreateElement(p_subName);
                                    p_subProNode.SetAttribute("Name", (string)m_dataTable.Rows[i]["SubProgramName"]);
                                    p_subProNode.SetAttribute("Link", (string)m_dataTable.Rows[i]["func_url"]);
                                    p_rootProNode.AppendChild(p_subProNode);
                                }
                                XmlElement p_proNode = p_xml.CreateElement(p_proId);
                                p_proNode.SetAttribute("Name", (string)m_dataTable.Rows[i]["func_name"]);
                                p_proNode.SetAttribute("Link", (string)m_dataTable.Rows[i]["func_url"]);
                                p_proNode.SetAttribute("NewTab", m_dataTable.Rows[i]["new_tab"].ToString());
                                p_subProNode.AppendChild(p_proNode);
                            }

                        }
                        p_userData.ProgramXml = p_xml.OuterXml;
                        //System.IO.File.WriteAllText("C:\\Test\\Program.xml", p_userData.ProgramXml, Encoding.UTF8);
                    }

                    p_userData.UserInfo = m_user;
                    p_userData.GroupList = m_groupList;
                    p_userData.CurrencyType = "T";

                    //check user_id if exist then update data status n then insert new 
                    m_sql = @"SELECT * FROM ZT_SysUserInfo 
WHERE user_id = @user_id AND status_flag = 'Y'";
                    m_paraList.Clear();
                    m_paraList.Add(new DBParameter("@user_id", p_userData.UserInfo.user_id));
                    m_dataTable = g_dba.GetDataTable(m_sql, m_paraList.ToArray());
                    if (m_dataTable.Rows.Count > 0)
                    {
                        m_sql = @"UPDATE ZT_SysUserInfo SET status_flag = 'N', 
updated_date=getdate() WHERE user_id = @user_id AND status_flag = 'Y'";
                        g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());
                    }
                    m_sql = @"INSERT INTO ZT_SysUserInfo (user_info_uuid,user_id,
user_name,org_id,role_id,status_flag,created_by,created_date,updated_by,updated_date) VALUES 
(@user_info_uuid,@user_id,@user_name,@org_id,@role_id,@status_flag,@created_by,getdate(),
@updated_by,getdate())";
                    m_paraList.Clear();
                    Guid m_token = Guid.NewGuid();
                    m_paraList.Add(new DBParameter("@user_info_uuid", m_token));
                    m_paraList.Add(new DBParameter("@user_id", p_userData.UserInfo.user_id));
                    m_paraList.Add(new DBParameter("@user_name", p_userData.UserInfo.user_name));
                    m_paraList.Add(new DBParameter("@org_id", p_userData.UserInfo.org_id));
                    m_paraList.Add(new DBParameter("@role_id", p_userData.UserInfo.role_id));
                    m_paraList.Add(new DBParameter("@status_flag", "Y"));
                    m_paraList.Add(new DBParameter("@created_by", p_userData.UserInfo.user_uuid));
                    m_paraList.Add(new DBParameter("@updated_by", p_userData.UserInfo.user_uuid));
                    g_dba.ExecNonQuery(m_sql, m_paraList.ToArray());

                    p_userData.LoginToken = m_token;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return p_userData;
        }
    }
}

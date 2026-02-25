'-------------------------------------------------------------------------------------------------------------------------------------------------------'
'SCREEN NAME :   Commonconstants.vb
'========================================================================================================================================================
'DESCRIPTION: : CommonConstants class for the ICMS system. 
'========================================================================================================================================================
'CMR No : 2016/03/15/J1
'--------------------------------------------------------------------------------------------------------------------------------------------------------
'Job Log No: 
'--------------------------------------------------------------------------------------------------------------------------------------------------------
'Version    Date            Name                        Change                      Work Done
'--------------------------------------------------------------------------------------------------------------------------------------------------------
'1.0       24/04/2016      Sumeet Singh            Initial Creation
Imports Microsoft.VisualBasic

Public Class CommonConstants
    Public Const SESSION_LIST_USER_ACCESS As String = "list_user_access"
    Public Const SESSION_USER_ID As String = "USER_ID"
    Public Const SESSION_USER_NAME As String = "USER_NAME"



    Public Const SESSION_ORG_ID As String = "orgID"
    Public Const SESSION_ORG_SHORT_NAME As String = "orgAlias"
    Public Const SESSION_ORG_LONG_NAME As String = "orgName"
    Public Const SESSION_FIN_YEAR As String = "fYear"
    Public Const SESSION_MONTH As String = "month"
    Public Const SESSION_RISK_ID As String = "riskID"
    Public Const SESSION_MEETING_IDS As String = "idList"
    Public Const SESSION_SELECTED_GROUP As String = "selected_group"
    Public Const SESSION_ROLE_IDS As String = "role_ids"
    Public Const SESSION_EDIT_AUTH As String = "edit_auth"
    Public Const RISK_ICON As String = "fa fa-exclamation-triangle font-red"
    Public Const PARENT_CAUSE_ICON As String = "fa fa-search font-blue"
    Public Const CHILD_CAUSE_ICON As String = "fa fa-search font-blue"
    Public Const ROOT_ORG_ICON As String = "fa fa-university font-blue"
    Public Const PARENT_ORG_ICON As String = "fa fa-diamond font-yellow-gold"
    Public Const CHILD_ORG_ICON As String = "fa fa-leaf font-green"
    Public Const ERM_DATE_FORMAT As String = "dd/MM/yyyy"
    Public Const ROOT_NODE As String = "R"
    Public Const NORMAL_NODE As String = "P"
    Public Const LEAF_NODE As String = "L"
    Public Const CAUSE_NODE As String = "C"
    Public Const CodeForInitial As String = "13"
    Public Const CodeForResidual As String = "14"
    Public Const CodeForLogged As String = "9"
    Public Const CodeTypeForLogged As String = "LOGGED"

    Public Const CodeForABPAssumption As String = "A"
    Public Const CodeForRiskAssumption As String = "R"
    Public Const CodeForOtherAssumption As String = "O"
    Public Const DANGER_COLOR_CODE_CAL As String = "#F64747"
    Public Const SUCCESS_COLOR_CODE_CAL As String = "#1BBC9B"
    Public Const WARN_COLOR_CODE_CAL As String = "#f6bc33"
    Public Const DEFAULT_COLOR_CODE_CAL As String = "#69a4e0"
    Public Const ATTACH_CATG_TYPE As String = "ATTACHMENT TYPE"
    Public Const MOM_TYPE As String = "MOM"
    Public Const REV_REPO_TYPE As String = "REVREPO"
    Public Const DEF_ROLE_ID As String = "1"
    Public Const SUPER_ADMIN_ROLE_ID As String = "5"
    Public Const ADMIN_ROLE_ID As String = "4"
    Public Const RISK_STATUS_LOGGED As String = "LOGGED"
    Public Const RISK_STATUS_APPROVED As String = "APPROVED"
    Public Const RISK_STATUS_REJECTED As String = "REJECTED"
    Public Const RISK_STATUS_RETURNED As String = "RETURNED"
    Public Const ATTACHMENT_DOWNLOAD_URL As String = "DownloadAttach.aspx?" & ATTACHMENT_DWNLD_REQ_VAR & "="
    Public Const ATTACHMENT_DWNLD_REQ_VAR As String = "id"
    Public Const MANAGERISK_URL As String = "ManageRisk.aspx?" & MANAGERISK_REQ_VAR & "="
    Public Const MANAGERISK_REQ_VAR As String = "ri"
    Public Const FAQ_URL As String = "faqs.aspx"
    Public Const TSG_ID As String = "1"
    Public Const ERM_TEAM_CODE_TYPE As String = "ERM"
    Public Const IT_TEAM_CODE_TYPE As String = "IT"
    Public Const RISK_COORDINATORS_CODE_TYPE As String = "ADMN"
    Public Const GLOBAL_ROLE As String = "GLBL"
    Public Const GLOBAL_ROLE_ID As String = "26"
    Public Const GREEN As String = "GREEN"
    Public Const RED As String = "RED"
    Public Const YELLOW As String = "YELLOW"
    Public Const GRAY As String = "GRAY"
    Public Const UNAUTHORIZED As String = "UnAuthorized"

    Public Const Active As String = "N"
    Public Const Deleted As String = "Y"
    Public Const Inactive As String = "I"
    Public Const QuarterCodeType As String = "QTR"
    Public Const UpdateDOButtonText As String = "Update DO Number"
    Public Const SabashDiwasIDPrefix As String = "SD/"
    Public Const SabashDiwasCouponPrefix As String = "C/SD/"
    Public Const InstantSabashIDPrefix As String = "IN/"
    Public Const InstantSabashCouponPrefix As String = "C/IN/"

    Public Const EditAction As String = "Update Diwas Details"
    Public Const DeleteAction As String = "Delete Diwas Details"

    Public Const EditAwardee As String = "Update Awardee Details"
    Public Const DeleteAwardee As String = "Delete Awardee Details"

    Public Const AppreciationText As String = "Good performance in your work area."

    Public Enum AlertType
        info
        success
        warning
        [error]
    End Enum



End Class

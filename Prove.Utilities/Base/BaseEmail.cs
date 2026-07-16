using System;
using System.Collections.Generic;
using System.Text;

namespace Prove.Utilities.Base
{
    public static class BaseEmail
    {
        public static string CreateMessage(string body) =>
            $@"<table width=""100%"" cellpadding=""0"" border=""0"" cellspacing=""0"">
                <tbody>
                    <tr>
                        <td align=""center"" bgcolor=""#EAECED"">
                            <table width=""580"" cellpadding=""0"" border=""0"" cellspacing=""0"" align=""center"" style=""width:580px;"">
                                <tbody>
                                    <tr>
                                        <td height=""25""></td>
                                    </tr>
                                </tbody>
                            </table>
                            <table width=""580"" cellpadding=""0"" border=""0"" cellspacing=""0"" align=""center""
                                style=""width:580px;border-radius:content_radius;margin-bottom: 25px;"" bgcolor=""#ffffff"">
                                <tbody>
                                    <tr>
                                        <td align=""left"">
                                            <img src=""https://www.ptk-shipping.com/assets/img/logo.png""
                                                style=""display: block; max-width: 256px; margin-top: 24px; margin-bottom: 24px""
                                                border=""0"" tabindex=""0"">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""padding-left:50px;padding-right:50px;padding-top:24px"">
                                            <table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                                                <tbody>{body}</tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table width=""580"" cellpadding=""0"" border=""0"" cellspacing=""0"" align=""center""
                                style=""min-width:580px;width:580px"">
                                <tbody>
                                    <tr>
                                        <td height=""25""></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>";

        public static string SCProductApprovedToVendor(string VendorName, string ProductName) =>
             $@"<p style='text-align:justify;'>Kepada Yth, </p>
                <br/> 
                <p style='text-align:justify;'>{VendorName}</p>
                <br/><br/>
                <p style='text-align:justify;'>Produk {ProductName} dari {VendorName} telah disetujui oleh verifikator ShipChandler dan produk tersebut telah masuk ke dalam katalog toko Anda.</p>
                <br/>
                <p style='text-align:justify;'>Terima kasih</p>";

        public static string SCProductApprovedToCrewing(string VendorName, string ProductName) =>
             $@"<p style='text-align:justify;'>Kepada Yth, </p>
                <br/> 
                <p style='text-align:justify;'>Fungsi Crewing PT Pertamina International Shipping</p>
                <br/><br/>
                <p style='text-align:justify;'>Penambahan/perubahan produk {ProductName} dari {VendorName} telah disetujui. Produk tersebut telah masuk ke dalam katalog toko di ShipChandler.</p>
                <br/>
                <p style='text-align:justify;'>Terima kasih</p>";

        public static string SCProductRejectedToVendor(string VendorName, string ProductName, string Reason) =>
             $@"<p style='text-align:justify;'>Kepada Yth, </p>
                <br/> 
                <p style='text-align:justify;'>{VendorName}</p>
                <br/><br/>
                <p style='text-align:justify;'>Produk {ProductName} dari {VendorName} tidak disetujui oleh verifikator ShipChandler karena “{Reason}”. Silahkan lakukan perbaikan agar produk dapat dimasukkan ke dalam katalog toko Anda. <a href='https://shipchandler.ptk-shipping.com/Product'>Klik disini</a> untuk menuju daftar produk toko.</p>
                <br/>
                <p style='text-align:justify;'>Terima kasih</p>";

        public static string SCConfirmEBastToShip(string ShipUser, string OrderNumber, string AdditionalFee, string link) =>
             $@"<p style='text-align:justify;'>Kepada Yth, </p>
                <br/> 
                <p style='text-align:justify;'>{ShipUser}</p>
                <br/><br/>
                <p style='text-align:justify;'>Pesanan dengan nomor {OrderNumber} telah ditambahkan additional fee oleh PTK/PKG sebesar Rp {AdditionalFee}. Silahkan konfirmasi terkait penambahan additional fee untuk menerbitkan E-Bast dari pesanan ini di aplikasi ShipChandler. <a href='{link}'>Klik disini</a> untuk konfirmasi handling fee dan transportation fee order.</p>
                < br/>
                <p style='text-align:justify;'>Terima kasih</p>";

        public static string IvendzMailToVendor(string InvoiceNumber, string CompanyName, string State, string Link, string reason) =>
            $@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">Notifikasi Progress Invoice No. {InvoiceNumber}</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Yang Terhormat {CompanyName},</p>
                                <p>Saat ini proses pembayaran invoice dengan No. {InvoiceNumber} sedang berada pada posisi {State} disebabkan {reason}, untuk mengevaluasi invoice terkait dapat dilihat <a href='{Link}'>disini</a>.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>";

        public static string IvendzMailToVendor(string InvoiceNumber, string CompanyName, string State, string Link) =>
            $@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">Notifikasi Progress Invoice No. {InvoiceNumber}</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Yang Terhormat {CompanyName},</p>
                                <p>Saat ini proses pembayaran invoice dengan No. {InvoiceNumber} sedang berada pada posisi {State}, untuk mengevaluasi invoice terkait dapat dilihat <a href='{Link}'>disini</a>.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>";

        public static string IvendzMailToPIC(string InvoiceNumber, string State, string Link, string reason) =>
            $@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">Notifikasi Progress Invoice No. {InvoiceNumber}</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Dengan Hormat,</p>
                                <p>Saat ini proses pembayaran invoice dengan No. {InvoiceNumber} sedang berada pada posisi {State} disebabkan {reason}, untuk data invoice terkait dapat dilihat <a href='{Link}'>disini</a>.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>";

        public static string IvendzMailToPIC(string InvoiceNumber, string State, string Link) =>
            $@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">Notifikasi Progress Invoice No. {InvoiceNumber}</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Dengan Hormat,</p>
                                <p>Saat ini proses pembayaran invoice dengan No. {InvoiceNumber} sedang berada pada posisi {State}, untuk data invoice terkait dapat dilihat <a href='{Link}'>disini</a>.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>";

        public static string VendorRejectAdministrationPhase(string vendorName) =>
            CreateMessage($@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">PRAKUALIFIKASI CSMS</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Yang Terhormat, {vendorName}</p>
                                <p>saat ini SKT saudara ada pada status Rejected, untuk selengkapnya check <a href='https://extra.ptk-shipping.com/Partner/PartnerIndex' target='_blank'>disini</a>.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>");

        public static string VendorApproveAdministrationPhase(string vendorName) =>
           CreateMessage($@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">PRAKUALIFIKASI CSMS</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Yang Terhormat, {vendorName}</p>
                                <p>saat ini SKT saudara ada pada status Approved, untuk selengkapnya check <a href='https://extra.ptk-shipping.com/Partner/PartnerIndex' target='_blank'>disini</a>.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>");

        public static string VendorPassword(string vendorName, string noNPWP, string password) =>
            CreateMessage($@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">Password Perusahaan</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Yang Terhormat, {vendorName}</p>
                                <p>Anda menerima sandi perusahaan untuk mendaftarkan akun anda pada aplikasi EXTRA.</p>
                                <div style=""padding-left:20px;padding-top:10px;padding-bottom:10px;padding-right:10px;background-color:#eaeced;margin-bottom:30px;border-radius:4px;line-height:160%"">
                                    Nomor NPWP: <b>{noNPWP}</b> <br>
                                    Sandi Perusahaan: <b>{password}</b>
                                </div>
                                <p>Sandi perusahaan ini diberikan untuk membuat akun pengguna pribadi baru dalam pengelolaan data perusahaan anda untuk Pertamina Trans Kontinental, pada <a href='https://extra.ptk-shipping.com/Partner/PartnerIndex' target='_blank'>Extra</a></p>
                                <p>please save and maintain it well</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>");


        public static string PresenceMail(string employeeName)
        {
            string greet = DateTime.Now.Hour < 12 ? "Semangat Pagi!" : "Semangat Sore!";
            return CreateMessage($@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33""><strong>Hi {employeeName},</br>{greet}</strong></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Terimakasih sudah mengisi form absen online digital PTK yang disubmit pada <strong>{DateTime.Now.ToString("dd MMM yyyy HH:mm:ss")}</strong>,</p>
                                <p>Harap simpan email ini sebagai bukti absen.</p>
                                <p><strong>#newnormal</br> #pakaimasker</br> #socialdistancing</br> #staysafe</br> #keepproductive</br> #ptkluarbiasa</br></strong></p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>
            ");
        }

        public static string RegulationRequestMail(string header, string body)
        {
            return CreateMessage($@"
                <tr><td height='20'></td></tr>
                <tr>
                    <td align='center' style='font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left'>
                        {body}
                    </td>
                </tr>
                <tr><td height='15'></td></tr>
                <tr><td height='25'></td></tr>
            ");
        }

        public static string RegulationReminderMail(string name, string dateExpired, string objectType, string objectName, string regulationName, string id)
        {
            return CreateMessage($@"
                <tr><td height='20'></td></tr>
                <tr>
                    <td align='center' style='font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left'>
                        <div style='margin-top:0px;margin-bottom:10px;text-align:left'>
                            <p>Dengan Hormat</p>
                            <p>
                                Masa Berlaku {regulationName} untuk {objectType} {objectName} akan segera habis pada tanggal {dateExpired}.
                                <br>
                                Mohon untuk dapat segera ditindaklanjuti dengan mengakses <a href='https://pride.ptk-shipping.com/RegulationResult/AddEdit/{id}'> link berikut</a>.  
                            </p>
                            <p>Terima Kasih</p>
                        </div>
                    </td>
                </tr>
                <tr><td height='15'></td></tr>
                <tr><td height='25'></td></tr>
            ");
        }
        public static string RegulationSLAMail(string name, string dateExpired, string objectType, string objectName, string regulationName, string id, string sisa)
        {
            return CreateMessage($@"
                <tr><td height='20'></td></tr>
                <tr>
                    <td align='center' style='font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left'>
                        <div style='margin-top:0px;margin-bottom:10px;text-align:left'>
                            <p>Dengan Hormat</p>
                            <p>
                                SLA Pengerjaan sertifikat {regulationName} untuk {objectType} {objectName} estimasi tanggal {dateExpired}, sisa {sisa} hari.
                                <br>
                                Mohon untuk dapat segera ditindaklanjuti.  
                            </p>
                            <p>Terima Kasih</p>
                        </div>
                    </td>
                </tr>
                <tr><td height='15'></td></tr>
                <tr><td height='25'></td></tr>
            ");
        }

        public static string ELearningNewDiscussion(string empName, string learningSessionTitle, string url)
        {
            return "<table width=\"100%\" cellpadding=\"0\" border=\"0\" cellspacing=\"0\" style=\"background-color:#FFFFFF;\"><tbody><tr><td align=\"center\" bgcolor=\"#EAECED\"><table width=\"580\" cellpadding=\"0\" border=\"0\" cellspacing=\"0\" align=\"center\" style=\"width:580px\">" +
                    "<tbody><tr><td height=\"25\"></td></tr></tbody></table><table width=\"580\" cellpadding=\"0\" border=\"0\" cellspacing=\"0\" align=\"center\" style=\"width:580px;border-radius:content_radius\" bgcolor=\"#ffffff\"><tbody><tr><td align=\"left\">" +
                    "<img src=\"https://www.ptk-shipping.com/assets/img/logo.png\" style=\"display: block; max-width: 256px; margin-top: 24px; margin-bottom: 24px\" border=\"0\" tabindex=\"0\"></td></tr><tr><td style=\"padding-left:50px;padding-right:50px;padding-top:24px\">" +
                    "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tbody><tr><td align=\"left\" style=\"font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800\"><p style=\"text-decoration:none;color:#252b33\">" +
                    "<span style=\"color:#252b33\">Bukti Absen Meeting Digital</span></p></td></tr><tr><td height=\"20\"></td></tr><tr><td align=\"center\" style=\"font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left\">" +
                    "<div><div style=\"margin-top:0px;margin-bottom:10px;text-align:left\"><p>Halo " + empName + ",</p><p>terdapat diskusi baru pada learning session dengan judul <b>" + learningSessionTitle + "</b>. Klik <a href=\"" + url + "\">link</a> untuk menuju learning session tersebut. </p>" +
                    "<p>Terima kasih</p></div></div></td></tr><tr><td height=\"15\"></td></tr><tr><td height=\"25\"></td></tr></tbody></table></td></tr></tbody></table><table width=\"580\" cellpadding=\"0\" border=\"0\" cellspacing=\"0\" align=\"center\" style=\"min-width:580px;width:580px\">" +
                    "<tbody><tr><td height=\"25\"></td></tr></tbody></table></td></tr></tbody></table>";
        }

        public static string BOSVDRHTToApprover(string vesselName, string confirmUrl)
            => CreateMessage($@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">VDR HT Notification</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Dh,</p>
                                <p>Hi Perwira</p>
                                <p>Anda mendapatkan update VDRHT Task dari {vesselName}</p>
                                <p>Segera lakukan <i>review</i> laporan dan pastikan data berikut
                                    sudah sesuai:</p>
                                <p>
                                <ol>
                                    <li>Pergerakan actual aktivitas kapal.</li>
                                    <li><i>Previous days</i> ROB Sounding dan <i>Open Stock</i>
                                        sama.</li>
                                    <li>Jika nilai discrepancy bernilai minus atau table view
                                        berwarna merah agar mengupload file <i>evidence</i> atau
                                        justifikasi dari kapal.</li>
                                </ol>
                                </p>
                                <p>Untuk melakukan review dan approval pada module Bunker
                                    Operation System (BOS), silahkan klik <a
                                        href=""{confirmUrl}"">disini</a>. <br>
                                    Untuk
                                    informasi lebih lanjut dapat menghubungi alamat email <a
                                        href=""mailto:BOS.PTK@pertamina.com"">BOS.PTK@pertamina.com</a>
                                </p>
                                <p>Demikian disampaikan, atas perhatian dan kerjasamanya kami
                                    ucapkan terima kasih.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>
            ");

        public static string BOSVDRHTToCrew(string vesselName, string status, string reportDate, string revisionUrl)
        {
            string labelColor = "";
            string additionalText = "";
            string checkText = "";
            switch (status)
            {
                case "APPROVED":
                    labelColor = "green";
                    checkText = $@"<p>Untuk melihat status terakhir pada aplikasi Bunker Operation System
                                    (BOS), silahkan klik <a href=""{revisionUrl}"">disini</a>.
                                </p>";
                    break;
                case "REJECTED":
                    labelColor = "red";
                    additionalText = $@"<br>
                                    Harap melakukan pengiriman ulang dan revisi VDR HT Task
                                    anda. Untuk informasi lebih lanjut dapat menghubungi PIC
                                    Port masing-masing.";
                    checkText = $@"<p>Untuk melakukan revisi pada aplikasi Bunker Operation System
                                    (BOS), silahkan klik <a href=""{revisionUrl}"">disini</a>.
                                </p>";
                    break;
            }
            return CreateMessage($@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">VDR HT Notification</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Dh,</p>
                                <p>Laporan VDR HT Task anda untuk kapal {vesselName} pada
                                    tanggal {reportDate} di <span
                                        style=""padding: 0.2rem;border-radius: 0.2rem;font-weight: 600;font-size: 0.9em; background-color: {labelColor};color: white;"">{status}</span>.
                                    {additionalText}
                                </p>
                                {checkText}
                                <p>Terima kasih atas perhatian dan kerjasamanya.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>
            ");
        }

        public static string BOSBunkerReqToApprover(string vesselName, string planDate, string approverUrl)
            => CreateMessage($@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">Bunker Request Notification</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Dh,</p>
                                <p>Hi Perwira</p>
                                <p>Anda mendapatkan permintaan Supply Bunker dari {vesselName} untuk tanggal {planDate}</p>
                                <p>Segera lakukan evaluasi untuk kemudian dapat di proses :</p>
                                <p>
                                <ol>
                                    <li>Draft Memo permintaan Bunker dengan melampirkan
                                        dokumen pada menu Memo List dan diteruskan melalui
                                        P-office dengan approval sesuai otorisasi dan dapat
                                        diteruskan kepada Pertamina Patra Niaga untuk dilakukan
                                        supply bunker.</li>
                                    <li>Mengupload kembali seluruh dokumen supply bunker pada
                                        aplikasi BOS max H+3 Supply bunker.</li>
                                </ol>
                                </p>
                                <p>Untuk melakukan approval pada module Bunker
                                    Operation System (BOS), silahkan klik <a
                                        href=""{approverUrl}"">disini</a>. <br>
                                    Untuk
                                    informasi lebih lanjut dapat menghubungi alamat email <a
                                        href=""mailto:BOS.PTK@pertamina.com"">BOS.PTK@pertamina.com</a>
                                </p>
                                <p>Demikian disampaikan, atas perhatian dan kerjasamanya kami
                                    ucapkan terima kasih.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>
            ");

        public static string BOSBunkerReqRejectToCrew(string vesselName, string planDate, string reviseUrl)
            => CreateMessage($@"
                <tr>
                    <td align=""left""
                        style=""font-family:'Inter',sans-serif;color:#213F57;font-size:24px;line-height:30px;font-weight:800"">
                        <p style=""text-decoration:none;color:#252b33"">
                            <span style=""color:#252b33"">Bunker Request Notification</span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height=""20""></td>
                </tr>
                <tr>
                    <td align=""center""
                        style=""font-family:'Inter',sans-serif;color:#2F3941;font-size:16px;line-height:22px;text-align:left"">
                        <div>
                            <div style=""margin-top:0px;margin-bottom:10px;text-align:left"">
                                <p>Dh,</p>
                                <p>Permintaan bunker anda pada tanggal {planDate} untuk kapal
                                    {vesselName} di <span
                                        style=""padding: 0.2rem;border-radius: 0.2rem;font-weight: 600;font-size: 0.9em; background-color: red;color: white;"">REJECT</span>,
                                    harap melakukan permintaan bunker ulang. Untuk informasi
                                    lebih lanjut dapat menghubungi PIC Port masing-masing.
                                </p>
                                <p>Untuk melakukan revisi pada aplikasi Bunker Operation System
                                    (BOS), silahkan klik <a href=""{reviseUrl}"">disini</a>.
                                </p>
                                <p>Terima kasih atas perhatian dan kerjasamanya.</p>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height=""15""></td>
                </tr>
                <tr>
                    <td height=""25""></td>
                </tr>
            ");
    }
}
